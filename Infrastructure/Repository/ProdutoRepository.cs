using Dapper;
using Domain.Entities;
using Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IDbConnection dbConnection;
        private bool disposedValue;

        public ProdutoRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }


        public async Task<IEnumerable<Produto>> ListAsync()
        {
            return await dbConnection.QueryAsync<Produto>("EXEC MostrarProdutos");
        }

        public async Task<Produto> GetAsync(int code)
        {
            return await dbConnection.QueryFirstOrDefaultAsync<Produto>("EXEC MostrarProdutoEspecifico @id", new { id = code });
        }

        public async Task<IEnumerable<Produto>> SelectAsync(Produto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Produto> CreateAsync(Produto entity)
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
                dynamic.Add("@Preco", entity.Preco);
                dynamic.Add("@idCategoria", entity.Id_Categoria);

                var affectedRows = await dbConnection.ExecuteAsync("CriarProduto", dynamic, commandType: CommandType.StoredProcedure);

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

        public async Task<Produto> UpdateAsync(Produto entity)
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
                dynamic.Add("@id", entity.Id_Produto);
                dynamic.Add("@Nome", entity.Nome);
               
                dynamic.Add("@ImagemUrl", entity.ImagemUrl);
                dynamic.Add("@Preco", entity.Preco);
                dynamic.Add("@idCategoria", entity.Id_Categoria);

                var affectedRows = await dbConnection.ExecuteAsync("UpProduto", dynamic, commandType: CommandType.StoredProcedure);

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

        public async Task<Produto> DeleteAsync(int code /*, int user*/)
        {
            var entity = new Produto();
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


                var affectedRows = await dbConnection.ExecuteAsync("deletarProduto", dynamic, commandType: CommandType.StoredProcedure);

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
