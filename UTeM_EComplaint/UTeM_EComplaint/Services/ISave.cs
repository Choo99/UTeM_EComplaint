using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UTeM_EComplaint.Services
{
    public interface ISave
    {
        Task SaveAndView(string fileName, String contentType, MemoryStream stream);
    }
}
