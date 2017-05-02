using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.IO.Interface
{
    public interface IFile
    {
        string FileGUID { get; set; }
        byte[] FileBytes { get; set; }
        string FileExtension { get; set; }

    }
}
