using BBP.IO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP.IO.Extensions
{
    public static class FileExtensions
    {
        public static void SaveFoto(this IFile file, string path)
        {
            if ((file == null)
                || (file.FileBytes == null)
                || (file.FileBytes.Length == 0)
                || (String.IsNullOrEmpty(file.FileGUID))
                || (String.IsNullOrEmpty(file.FileExtension))
              )
                throw new Exception("Para salvar a foto, todos os elementos de IFile devem ser preenchidos");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!Directory.Exists(path))
                throw new Exception($"Caminho {path} inexistente para salvar a foto");

            file.FileExtension = file.FileExtension.Replace(".", "");

            //ToDo repassar para o Franework
            File.WriteAllBytes($"{path}\\.{file.FileGUID}.{file.FileExtension}", file.FileBytes);
        }

    }
}
