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
                //Par�metros passados para a procedure

                //Campos base
                //C�digo do registro inserido / Usu�rio que inseriu / Data e hora da inclus�o
                //dynamic.Add("@CODIGO", dbType: DbType.Int32, direction: ParameterDirection.Output);
                //dynamic.Add("@USUINC", entity.User);
                //dynamic.Add("@DATINC", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                //Campos do neg�cio
                dynamic.Add("@Nome", entity.Nome);
                dynamic.Add("@ImagemUrl", entity.ImagemUrl);
                dynamic.Add("@Preco", entity.Preco);
                dynamic.Add("@idCategoria", entity.Id_Categoria);

                var affectedRows = await dbConnection.ExecuteAsync("CriarProduto", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Par�metros de retorno
                    //
                    //C�digo inserido (@@IDENTITY)
                    //entity.Code = dynamic.Get<int>("CODIGO");
                    ////Data e hora de inclus�o do registro
                    //entity.DatInc = dynamic.Get<DateTime>("DATINC");
                }
                else
                {
                    throw new Exception("Registro(s) n�o foi(ram) alterado(s)!");
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
                //Par�metros passados para a procedure
                //
                //Par�metro chave para a altera��o
                //dynamic.Add("@CODIGO", entity.Code);

                ////Campos base
                ////Usu�rio que alterou / Data e hora da altera��o
                //dynamic.Add("@USUALT", entity.User);
                //dynamic.Add("@DATALT", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                //Campos do neg�cio
                dynamic.Add("@id", entity.Id_Produto);
                dynamic.Add("@Nome", entity.Nome);
               
                dynamic.Add("@ImagemUrl", entity.ImagemUrl);
                dynamic.Add("@Preco", entity.Preco);
                dynamic.Add("@idCategoria", entity.Id_Categoria);

                var affectedRows = await dbConnection.ExecuteAsync("UpProduto", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Par�metros de retorno
                    //
                    //Data e hora da altera��o do registro
                    //entity.DatInc = dynamic.Get<DateTime>("DATALT");
                }
                else
                {
                    throw new Exception("Registro(s) n�o foi(ram) alterado(s)!");
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
                //Par�metro chave da altera��o

                //Campos base
                //Usu�rio que excluiu / Data e hora da exclus�o
                //dynamic.Add("DATDEL", dbType: DbType.DateTime, direction: ParameterDirection.Output);

                ////Par�metro chave para a exclus�o
                //dynamic.Add("CODIGO", code);
                //dynamic.Add("USUDEL", user);
                dynamic.Add("@id", code);


                var affectedRows = await dbConnection.ExecuteAsync("deletarProduto", dynamic, commandType: CommandType.StoredProcedure);

                if (affectedRows > 0)
                {
                    //Par�metros de retorno
                    //
                    //Data e hora da exclus�o do registro
                    //entity.DatDel = dynamic.Get<DateTime>("DATDEL");
                }
                else
                {
                    throw new Exception("Registro(s) n�o foi(ram) alterado(s)!");
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
