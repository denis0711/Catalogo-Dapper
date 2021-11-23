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
    public class ProdutoController : Controller
    {
        private readonly IProdutoService service;
        private readonly IMapper mapper;
        private readonly string chaveToken;

        public ProdutoController(IConfiguration configuration, IProdutoService service, IMapper mapper)
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
        /// Obtém um(a) Produto
        /// </summary>
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
        /// Retorna uma lista de Produto
        /// </summary>
        [HttpPatch]
        public async Task<IActionResult> Select(Domain.DTO.Produto entityIn)
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var entity = mapper.Map<Produto>(entityIn);

            var resultado = await service.SelectAsync(entity);
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }

        // POST api/todo
        /// <summary>
        /// Cria um(a) novo(a) Produto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Domain.DTO.Produto entityIn)
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var entity = mapper.Map<Produto>(entityIn);
            entity.User = autenticacao.CodigoUsuario;

            var resultado = await service.CreateAsync(entity);
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }


        // PUT api/todo
        /// <summary>
        /// Altera um(a) novo(a) Produto
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,Domain.DTO.Produto entityIn)
        {
            var autenticacao = Utils.ValidaToken(User.Claims, this.chaveToken);
            if (autenticacao == null)
                return Unauthorized();

            var entity = mapper.Map<Produto>(entityIn);
            entity.User = autenticacao.CodigoUsuario;

            var resultado = await service.UpdateAsync(entity);
            if (resultado.BadRequest)
                return new BadRequestObjectResult(resultado);

            return new ObjectResult(resultado.Conteudo);
        }
		
        // DELETE api/todo
        /// <summary>
        /// Exclui um(a) Produto
        /// </summary>
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
