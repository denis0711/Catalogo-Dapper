using Domain.Base;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using FluentValidation;
using Infrastructure.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Service.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProdutoService : IProdutoService
    {
        public readonly IProdutoRepository infrastructure;
        private readonly IValidator<Produto> validator;
        private readonly RedisCacheExtensions cache;

        public ProdutoService(IDbConnection dbConnection, IValidator<Produto> validator, IDistributedCache cache)
        {
            infrastructure = new ProdutoRepository(dbConnection);
            this.validator = validator;
            this.cache = new RedisCacheExtensions(cache);
        }

        public async Task<Resultado<IEnumerable<Produto>>> ListAsync()
        {
            try
            {
                return Resultado<IEnumerable<Produto>>.ComSucesso(await this.infrastructure.ListAsync());
            }
            catch (Exception exception)
            {
                return Resultado<IEnumerable<Produto>>.ComErros(null, Resultado<IEnumerable<Produto>>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Produto>> GetAsync(int code)
        {
            try
            {
                return Resultado<Produto>.ComSucesso(await this.infrastructure.GetAsync(code));
            }
            catch (Exception exception)
            {
                return Resultado<Produto>.ComErros(null, Resultado<Produto>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<IEnumerable<Produto>>> SelectAsync(Produto entity)
        {
            try
            {
                return Resultado<IEnumerable<Produto>>.ComSucesso(await this.infrastructure.SelectAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<IEnumerable<Produto>>.ComErros(null, Resultado<IEnumerable<Produto>>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Produto>> CreateAsync(Produto entity)
        {
            try
            {
                var resultadoValidacao = validator.Validate(entity);
                

                if (!resultadoValidacao.IsValid)
                {
                    var errosValidacao = resultadoValidacao.Errors.Select(e => Error.Criar(e.PropertyName, e.ErrorMessage, TipoErro.Validacao, e.AttemptedValue)).ToList();
                    return Resultado<Produto>.ComErros(entity, errosValidacao);
                }



                return Resultado<Produto>.ComSucesso(await this.infrastructure.CreateAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<Produto>.ComErros(null, Resultado<Produto>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Produto>> UpdateAsync(Produto entity)
        {
            try
            {
                var resultadoValidacao = validator.Validate(entity);
                if (!resultadoValidacao.IsValid)
                {
                    var errosValidacao = resultadoValidacao.Errors.Select(e => Error.Criar(e.PropertyName, e.ErrorMessage, TipoErro.Validacao, e.AttemptedValue)).ToList();
                    return Resultado<Produto>.ComErros(entity, errosValidacao);
                }

                return Resultado<Produto>.ComSucesso(await this.infrastructure.UpdateAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<Produto>.ComErros(null, Resultado<Produto>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }
   
        public async Task<Resultado<Produto>> DeleteAsync(int code/*, int user*/)
        {
            try
            {
                return Resultado<Produto>.ComSucesso(await this.infrastructure.DeleteAsync(code/*, user*/));
            }
            catch (Exception exception)
            {
                return Resultado<Produto>.ComErros(null, Resultado<Produto>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
