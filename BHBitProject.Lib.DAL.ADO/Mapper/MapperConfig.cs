using BHBitProject.Lib.DAL.Interface.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.DAL.ADO
{
    public static class MapperConfig
    {
        /// <summary>
        /// Efetua o mapeamento do MicroORM utilizado pelo farmework pelo escolhido, o framework fornecido
        /// deverá implementar a interface IMicroORM e possuir um construtor padrão vazio
        /// </summary>
        /// <typeparam name="TypeUse"></typeparam>
        public static void ConfigureMicroORM<TypeUse>()
            where TypeUse : IMicroORM, new()
        {
            Configuration.SetMicroORM(new TypeUse());
        }
    }
}
