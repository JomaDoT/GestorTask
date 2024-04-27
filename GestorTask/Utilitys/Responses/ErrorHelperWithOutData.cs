using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorTask.Utilitys.Responses;

public class ErrorHelperWithOutData
{
    public static ModelResponse Response(int errCode, string msg)
    {
        return new ModelResponse
        {
            Code = errCode,
            Message = msg
        };
    }
}

public class ModelResponse
{
    public int Code { get; set; }

    public string Message { get; set; }
}
