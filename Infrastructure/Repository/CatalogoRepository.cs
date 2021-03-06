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
                //Par?metros passados para a procedure
                //
                //Campos base
                //C?digo do registro inserido / Usu?rio que inseriu / Data e hora da inclus?o
                dynamic.Add("@CODIGO", dbType: DbType.Int32, direction: ParameterDirection.Output);
                dynamic.Add("@USUINC", entity.User);
                dynamic.Add("@DATINC", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                //Campos do neg?cio
                dynamic.Add("@PARAM_DA_PROC_01", entity.Param01);
                dynamic.Add("@PARAM_DA_PROC_02", entity.Param02);

                var affectedRows = await dbConnection.ExecuteAsync("PDA_SP_TEMPLATE_I", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Par?metros de retorno
                    //
                    //C?digo inserido (@@IDENTITY)
                    entity.Code = dynamic.Get<int>("CODIGO");
                    //Data e hora de inclus?o do registro
                    entity.DatInc = dynamic.Get<DateTime>("DATINC");
                }
                else
                {
                    throw new Exception("Registro(s) n?o foi(ram) alterado(s)!");
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
                //Par?metros passados para a procedure
                //
                //Par?metro chave para a altera??o
                dynamic.Add("@CODIGO", entity.Code);

                //Campos base
                //Usu?rio que alterou / Data e hora da altera??o
                dynamic.Add("@USUALT", entity.User);
                dynamic.Add("@DATALT", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                //Campos do neg?cio
                dynamic.Add("@PARAM_DA_PROC_01", entity.Param01);
                dynamic.Add("@PARAM_DA_PROC_02", entity.Param02);

                var affectedRows = await dbConnection.ExecuteAsync("PDA_SP_TEMPLATE_U", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Par?metros de retorno
                    //
                    //Data e hora da altera??o do registro
                    entity.DatInc = dynamic.Get<DateTime>("DATALT");
                }
                else
                {
                    throw new Exception("Registro(s) n?o foi(ram) alterado(s)!");
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
                //Par?metro chave da altera??o

                //Campos base
                //Usu?rio que excluiu / Data e hora da exclus?o
                //dynamic.Add("DATDEL", dbType: DbType.DateTime, direction: ParameterDirection.Output);
                
                ////Par?metro chave para a exclus?o
                //dynamic.Add("CODIGO", code);
                //dynamic.Add("USUDEL", user);

                var affectedRows = await dbConnection.ExecuteAsync("PDA_SP_TEMPLATE_D", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Par?metros de retorno
                    //
                    //Data e hora da exclus?o do registro
                    entity.DatDel = dynamic.Get<DateTime>("DATDEL");
                }
                else
                {
                    throw new Exception("Registro(s) n?o foi(ram) alterado(s)!");
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
