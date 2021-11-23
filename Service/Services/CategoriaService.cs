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
    public class CategoriaService : ICategoriaService
    {
        public readonly ICategoriaRepository infrastructure;
        private readonly IValidator<Categoria> validator;
        private readonly RedisCacheExtensions cache;

        public CategoriaService(IDbConnection dbConnection, IValidator<Categoria> validator, IDistributedCache cache)
        {
            infrastructure = new CategoriaRepository(dbConnection);
            this.validator = validator;
            this.cache = new RedisCacheExtensions(cache);
        }

        public async Task<Resultado<IEnumerable<Categoria>>> ListAsync()
        {
            try
            {
                return Resultado<IEnumerable<Categoria>>.ComSucesso(await this.infrastructure.ListAsync());
            }
            catch (Exception exception)
            {
                return Resultado<IEnumerable<Categoria>>.ComErros(null, Resultado<IEnumerable<Categoria>>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Categoria>> GetAsync(int code)
        {
            try
            {
                return Resultado<Categoria>.ComSucesso(await this.infrastructure.GetAsync(code));
            }
            catch (Exception exception)
            {
                return Resultado<Categoria>.ComErros(null, Resultado<Categoria>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<IEnumerable<Categoria>>> SelectAsync(Categoria entity)
        {
            try
            {
                return Resultado<IEnumerable<Categoria>>.ComSucesso(await this.infrastructure.SelectAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<IEnumerable<Categoria>>.ComErros(null, Resultado<IEnumerable<Categoria>>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Categoria>> CreateAsync(Categoria entity)
        {
            try
            {
                var resultadoValidacao = validator.Validate(entity);
                if (!resultadoValidacao.IsValid)
                {
                    var errosValidacao = resultadoValidacao.Errors.Select(e => Error.Criar(e.PropertyName, e.ErrorMessage, TipoErro.Validacao, e.AttemptedValue)).ToList();
                    return Resultado<Categoria>.ComErros(entity, errosValidacao);
                }

                return Resultado<Categoria>.ComSucesso(await this.infrastructure.CreateAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<Categoria>.ComErros(null, Resultado<Categoria>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Categoria>> UpdateAsync(Categoria entity)
        {
            try
            {
                var resultadoValidacao = validator.Validate(entity);
                if (!resultadoValidacao.IsValid)
                {
                    var errosValidacao = resultadoValidacao.Errors.Select(e => Error.Criar(e.PropertyName, e.ErrorMessage, TipoErro.Validacao, e.AttemptedValue)).ToList();
                    return Resultado<Categoria>.ComErros(entity, errosValidacao);
                }

                return Resultado<Categoria>.ComSucesso(await this.infrastructure.UpdateAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<Categoria>.ComErros(null, Resultado<Categoria>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Categoria>> DeleteAsync(int code/*, int user*/)
        {
            try
            {
                return Resultado<Categoria>.ComSucesso(await this.infrastructure.DeleteAsync(code/*, user*/));
            }
            catch (Exception exception)
            {
                return Resultado<Categoria>.ComErros(null, Resultado<Categoria>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
