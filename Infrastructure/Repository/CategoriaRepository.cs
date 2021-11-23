using Dapper;
using Domain.Entities;
using Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly IDbConnection dbConnection;
        private bool disposedValue;

        public CategoriaRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }


        public async Task<IEnumerable<Categoria>> ListAsync()
        {
            return await dbConnection.QueryAsync<Categoria>("EXEC MostrarTodos");
        }

        public async Task<Categoria> GetAsync(int code)
        {
            return await dbConnection.QueryFirstOrDefaultAsync<Categoria>("EXEC MostrarCategoriaEspecifico @id", new { id = code });
        }

        public async Task<IEnumerable<Categoria>> SelectAsync(Categoria entity)
        {
            return await dbConnection.QueryAsync<Categoria>("EXEC PDA_SP_TEMPLATE_S");
        }

        public async Task<Categoria> CreateAsync(Categoria entity)
        {
            var dynamic = new DynamicParameters();
            try
            {
                //Parâmetros passados para a procedure

                //Campos base
                //Código do registro inserido / Usuário que inseriu / Data e hora da inclusão
                //dynamic.Add("@CODIGO", dbType: DbType.Int32, direction: ParameterDirection.Output);
                //dynamic.Add("@USUINC", entity.User);
                //dynamic.Add("@DATINC", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                //Campos do negócio
                dynamic.Add("@Nome", entity.Nome);
                dynamic.Add("@ImagemUrl", entity.ImagemUrl);

                var affectedRows = await dbConnection.ExecuteAsync("CriarCategoria", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Parâmetros de retorno
                    //
                    //Código inserido (@@IDENTITY)
                    //entity.Code = dynamic.Get<int>("CODIGO");
                    ////Data e hora de inclusão do registro
                    //entity.DatInc = dynamic.Get<DateTime>("DATINC");
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

        public async Task<Categoria> UpdateAsync(Categoria entity)
        {
            var dynamic = new DynamicParameters();
            try
            {
                //Parâmetros passados para a procedure
                //
                //Parâmetro chave para a alteração
                //dynamic.Add("@CODIGO", entity.Code);

                ////Campos base
                ////Usuário que alterou / Data e hora da alteração
                //dynamic.Add("@USUALT", entity.User);
                //dynamic.Add("@DATALT", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                //Campos do negócio
                dynamic.Add("@id", entity.Id_Categoria);
                dynamic.Add("@Nome", entity.Nome);
                dynamic.Add("@ImagemUrl", entity.ImagemUrl);
              

                var affectedRows = await dbConnection.ExecuteAsync("UpCategoria", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Parâmetros de retorno
                    //
                    //Data e hora da alteração do registro
                    //entity.DatInc = dynamic.Get<DateTime>("DATALT");
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

        public async Task<Categoria> DeleteAsync(int code/*, int user*/)
        {
            var entity = new Categoria();
            
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
                dynamic.Add("@id", code);


                var affectedRows = await dbConnection.ExecuteAsync("deletarCategoira", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Parâmetros de retorno
                    //
                    //Data e hora da exclusão do registro
                    //entity.DatDel = dynamic.Get<DateTime>("DATDEL");
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
