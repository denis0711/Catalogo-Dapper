using Application.AppData;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoController : Controller
    {
        private readonly ICatalogoService service;
        private readonly IMapper mapper;
        private readonly string chaveToken;

        public CatalogoController(IConfiguration configuration, ICatalogoService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.chaveToken = configuration.GetSection("Seguranca:ChaveToken").Value;
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var resultado = await service.ListAsync();
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }

        // GET api/todo
        /// <summary>
        /// Obtém um(a) Catalogo
        /// </summary>
		/*
        /// <remarks>
        /// Exemplo:
        ///
        /// GET / TODO
        ///
        ///
        /// </remarks>
        /// <param name="codigo">Código do(a) Catalogo</param>
        /// <returns>O(a) Catalogo pesquisado(a)</returns>
		*/
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(int codigo)
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var resultado = await service.GetAsync(codigo);
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }

        // PATCH api/todo
        /// <summary>
        /// Retorna uma lista de Catalogo
        /// </summary>
		/*
        /// <remarks>
        /// Exemplo:
        ///
        /// Observação de campo obrigatório: Qual(is) campo(s)?
        /// Outros campos não são obrigatórios podendo preencher apenas a informação desejada na pesquisa
        /// 
        /// PATCH / TODO
        ///
        ///     {
        ///         "Campo1": Valor1,
        ///         "Campo2": "Valor2"
        ///     }
        ///
        /// </remarks>
        /// <param name="entityIn">Objeto para realizar a consulta do(a) Catalogo</param>
        /// <returns>O(a) Catalogo pesquisado(a)</returns>
        /// <response code="200">Retorna badRequest: true para insucesso e Erros: Lista de erros</response>
		*/
        [HttpPatch]
        public async Task<IActionResult> Select(Domain.DTO.Catalogo entityIn)
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var entity = mapper.Map<Catalogo>(entityIn);

            var resultado = await service.SelectAsync(entity);
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }

        // POST api/todo
        /// <summary>
        /// Cria um(a) novo(a) Catalogo
        /// </summary>
		/*
        /// <remarks>
        /// Exemplo:
        ///
        /// POST / TODO
        ///
        ///     {
        ///         "Campo1": Valor1,
        ///         "Campo2": "Valor2"
        ///     }
        ///
        /// </remarks>
        /// <param name="entityIn">Objeto para realizar a inclusão do(a) Catalogo</param>
        /// <returns>O novo item criado</returns>
        /// <response code="200">Retorna badRequest: true para insucesso e Erros: Lista de erros</response>
		*/
        [HttpPost]
        public async Task<IActionResult> Create(Domain.DTO.Catalogo entityIn)
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var entity = mapper.Map<Catalogo>(entityIn);
            entity.User = autenticacao.CodigoUsuario;

            var resultado = await service.CreateAsync(entity);
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }


        // PUT api/todo
        /// <summary>
        /// Altera um(a) novo(a) Catalogo
        /// </summary>
		/*
        /// <remarks>
        /// Exemplo:
        ///
        /// PUT / TODO
        ///
        ///     {
        ///         "Campo1": Valor1,
        ///         "Campo2": "Valor2"
        ///     }
        ///
        /// </remarks>
        /// <param name="entityIn">Objeto para realizar a alteração do(a) Catalogo</param>
        /// <returns>O item alterardo</returns>
        /// <response code="200">Retorna badRequest: true para insucesso e Erros: Lista de erros</response>
		*/
        [HttpPut]
        public async Task<IActionResult> Update(Domain.DTO.Catalogo entityIn)
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var entity = mapper.Map<Catalogo>(entityIn);
            entity.User = autenticacao.CodigoUsuario;

            var resultado = await service.UpdateAsync(entity);
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }
		
        // DELETE api/todo
        /// <summary>
        /// Exclui um(a) Catalogo
        /// </summary>
		/*
        /// <remarks>
        /// Exemplo:
        ///
        /// POST / TODO
        ///
        ///     /api/Endereco?codigo="codigo" codigoCliente="codigoCliente" tipo="tipo" user="user"
        ///
        /// </remarks>
        /// <param name="codigo">Código a ser excluído</param>
        /// <returns>Item excluído com sucesso!</returns>
        /// <response code="200">Retorna badRequest: true para insucesso e Erros: Lista de erros</response>
		*/
        [HttpDelete]
        public async Task<IActionResult> Delete(int codigo)
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var resultado = await service.DeleteAsync(codigo/*, (int)autenticacao.CodigoUsuario*/);
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }
    }
}
