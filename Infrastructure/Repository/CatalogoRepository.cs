using Dapper;
using Domain.Entities;
using Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CatalogoRepository : ICatalogoRepository
    {
        private readonly IDbConnection dbConnection;
        private bool disposedValue;

        public CatalogoRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }


        public async Task<IEnumerable<Catalogo>> ListAsync()
        {
            return await dbConnection.QueryAsync<Catalogo>("EXEC MostrarTodos");
        }

        public async Task<Catalogo> GetAsync(int code)
        {
            return await dbConnection.QueryFirstOrDefaultAsync<Catalogo>("EXEC PDA_SP_TEMPLATE_S @PARAM_CODIGO", new { PARAM_CODIGO = code });
        }

        public async Task<IEnumerable<Catalogo>> SelectAsync(Catalogo entity)
        {
            
            return await dbConnection.QueryAsync<Catalogo>("EXEC PDA_SP_TEMPLATE_S @PARAM_USUARIO, @PARAM_DATINC", new { PARAM_DA_PROC_1 = entity.Param01, PARAM_DA_PROC_2 = entity.Param01 });
        }

        public async Task<Catalogo> CreateAsync(Catalogo entity)
        {
            var dynamic = new DynamicParameters();
            try
            {
                //Parâmetros passados para a procedure
                //
                //Campos base
                //Código do registro inserido / Usuário que inseriu / Data e hora da inclusão
                dynamic.Add("@CODIGO", dbType: DbType.Int32, direction: ParameterDirection.Output);
                dynamic.Add("@USUINC", entity.User);
                dynamic.Add("@DATINC", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                //Campos do negócio
                dynamic.Add("@PARAM_DA_PROC_01", entity.Param01);
                dynamic.Add("@PARAM_DA_PROC_02", entity.Param02);

                var affectedRows = await dbConnection.ExecuteAsync("PDA_SP_TEMPLATE_I", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Parâmetros de retorno
                    //
                    //Código inserido (@@IDENTITY)
                    entity.Code = dynamic.Get<int>("CODIGO");
                    //Data e hora de inclusão do registro
                    entity.DatInc = dynamic.Get<DateTime>("DATINC");
                }
                else
                {
                    throw new Exception("Registro(s) não foi(ram) alterado(s)!");
                }
            }            
            finally
            {
                dynamic = null;
            }

            return entity;
        }

        public async Task<Catalogo> UpdateAsync(Catalogo entity)
        {
            var dynamic = new DynamicParameters();
            try
            {
                //Parâmetros passados para a procedure
                //
                //Parâmetro chave para a alteração
                dynamic.Add("@CODIGO", entity.Code);

                //Campos base
                //Usuário que alterou / Data e hora da alteração
                dynamic.Add("@USUALT", entity.User);
                dynamic.Add("@DATALT", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                //Campos do negócio
                dynamic.Add("@PARAM_DA_PROC_01", entity.Param01);
                dynamic.Add("@PARAM_DA_PROC_02", entity.Param02);

                var affectedRows = await dbConnection.ExecuteAsync("PDA_SP_TEMPLATE_U", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Parâmetros de retorno
                    //
                    //Data e hora da alteração do registro
                    entity.DatInc = dynamic.Get<DateTime>("DATALT");
                }
                else
                {
                    throw new Exception("Registro(s) não foi(ram) alterado(s)!");
                }
            }
            finally
            {
                dynamic = null;
            }

            return entity;
        }

        public async Task<Catalogo> DeleteAsync(int code/*, int user*/)
        {
            var entity = new Catalogo();
            var dynamic = new DynamicParameters();
            try
            {
                //Parâmetro chave da alteração

                //Campos base
                //Usuário que excluiu / Data e hora da exclusão
                //dynamic.Add("DATDEL", dbType: DbType.DateTime, direction: ParameterDirection.Output);
                
                ////Parâmetro chave para a exclusão
                //dynamic.Add("CODIGO", code);
                //dynamic.Add("USUDEL", user);

                var affectedRows = await dbConnection.ExecuteAsync("PDA_SP_TEMPLATE_D", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Parâmetros de retorno
                    //
                    //Data e hora da exclusão do registro
                    entity.DatDel = dynamic.Get<DateTime>("DATDEL");
                }
                else
                {
                    throw new Exception("Registro(s) não foi(ram) alterado(s)!");
                }
            }
            finally
            {
                dynamic = null;
            }

            return entity;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {                    
                    dbConnection.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
