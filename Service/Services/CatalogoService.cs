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
    public class CatalogoService : ICatalogoService
    {
        public readonly ICatalogoRepository infrastructure;
        private readonly IValidator<Catalogo> validator;
        private readonly RedisCacheExtensions cache;

        public CatalogoService(IDbConnection dbConnection, IValidator<Catalogo> validator, IDistributedCache cache)
        {
            infrastructure = new CatalogoRepository(dbConnection);
            this.validator = validator;
            this.cache = new RedisCacheExtensions(cache);
        }

        public async Task<Resultado<IEnumerable<Catalogo>>> ListAsync()
        {
            try
            {
                return Resultado<IEnumerable<Catalogo>>.ComSucesso(await this.infrastructure.ListAsync());
            }
            catch (Exception exception)
            {
                return Resultado<IEnumerable<Catalogo>>.ComErros(null, Resultado<IEnumerable<Catalogo>>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Catalogo>> GetAsync(int code)
        {
            try
            {
                return Resultado<Catalogo>.ComSucesso(await this.infrastructure.GetAsync(code));
            }
            catch (Exception exception)
            {
                return Resultado<Catalogo>.ComErros(null, Resultado<Catalogo>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<IEnumerable<Catalogo>>> SelectAsync(Catalogo entity)
        {
            try
            {
                return Resultado<IEnumerable<Catalogo>>.ComSucesso(await this.infrastructure.SelectAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<IEnumerable<Catalogo>>.ComErros(null, Resultado<IEnumerable<Catalogo>>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Catalogo>> CreateAsync(Catalogo entity)
        {
            try
            {
                var resultadoValidacao = validator.Validate(entity);
                if (!resultadoValidacao.IsValid)
                {
                    var errosValidacao = resultadoValidacao.Errors.Select(e => Error.Criar(e.PropertyName, e.ErrorMessage, TipoErro.Validacao, e.AttemptedValue)).ToList();
                    return Resultado<Catalogo>.ComErros(entity, errosValidacao);
                }

                return Resultado<Catalogo>.ComSucesso(await this.infrastructure.CreateAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<Catalogo>.ComErros(null, Resultado<Catalogo>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Catalogo>> UpdateAsync(Catalogo entity)
        {
            try
            {
                var resultadoValidacao = validator.Validate(entity);
                if (!resultadoValidacao.IsValid)
                {
                    var errosValidacao = resultadoValidacao.Errors.Select(e => Error.Criar(e.PropertyName, e.ErrorMessage, TipoErro.Validacao, e.AttemptedValue)).ToList();
                    return Resultado<Catalogo>.ComErros(entity, errosValidacao);
                }

                return Resultado<Catalogo>.ComSucesso(await this.infrastructure.UpdateAsync(entity));
            }
            catch (Exception exception)
            {
                return Resultado<Catalogo>.ComErros(null, Resultado<Catalogo>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public async Task<Resultado<Catalogo>> DeleteAsync(int code/*, int user*/)
        {
            try
            {
                return Resultado<Catalogo>.ComSucesso(await this.infrastructure.DeleteAsync(code/*, user*/));
            }
            catch (Exception exception)
            {
                return Resultado<Catalogo>.ComErros(null, Resultado<Catalogo>.AdicionarErro(Error.Criar(string.Empty, $"{exception}", TipoErro.Excecao, null)));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
