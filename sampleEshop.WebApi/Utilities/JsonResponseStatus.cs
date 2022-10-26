using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleEshop.WebApi.Utilities
{
    public enum Status
    {
        Success = 1,
        Error = 2,
        NotFound = 3,
        UnAuthorized = 4,
        AccessDenied = 5,
        //SomethingWrong = 6
    }
    public static class JsonResponseStatus
    {
        public static JsonResult Success()
        {
            return new JsonResult(new { eStatus = Status.Success });
        }

        public static JsonResult Success(object returnData)
        {
            return new JsonResult(new { data = returnData, eStatus = Status.Success });
        }
        public static JsonResult Success(object returnData, string message)
        {
            return new JsonResult(new { data = returnData, message = message, eStatus = Status.Success });
        }
        //****************************************
        //public static JsonResult SomethingWrong()
        //{
        //    return new JsonResult(new { eStatus = Status.SomethingWrong });
        //}
        //public static JsonResult SomethingWrong(string message)
        //{
        //    return new JsonResult(new { message = message, eStatus = Status.SomethingWrong });
        //}
        //****************************************
        //public static JsonResult NotFound()
        //{
        //    return new JsonResult(new { eStatus = Status.NotFound });
        //}
        //public static JsonResult NotFound()
        //{
        //    return new JsonResult(new { message= "آیتم مورد نظر یافت نشد", eStatus = Status.NotFound });
        //}

        public static JsonResult NotFound(string message = "آیتم مورد نظر پیدا نشد")
        {
            return new JsonResult(new { message = message, eStatus = Status.NotFound });
        }

        //****************************************
        public static JsonResult Error()
        {
            return new JsonResult(new { eStatus = Status.Error });
        }

        //public static JsonResult Error(object returnData)
        //{
        //    return new JsonResult(new { data = returnData, eStatus = Status.Error });
        //}

        public static JsonResult Error(string message)
        {
            return new JsonResult(new { eStatus = Status.Error, message = message });
        }

        //****************************************
        public static JsonResult UnAuthorized()
        {
            return new JsonResult(new { status = "UnAuthorized", eStatus = Status.UnAuthorized });
        }
        public static JsonResult UnAuthorized(object returnData)
        {
            return new JsonResult(new { data = returnData, eStatus = Status.UnAuthorized });
        }

        //****************************************
        public static JsonResult AccessDenied()
        {
            return new JsonResult(new { eStatus = Status.AccessDenied });
        }
        public static JsonResult AccessDenied(object returnData)
        {
            return new JsonResult(new { data = returnData, eStatus = Status.AccessDenied });
        }


        //public static JsonResult Success()
        //{
        //    return new JsonResult(new { status = "Success" });
        //}

        //public static JsonResult Success(object returnData)
        //{
        //    return new JsonResult(new { status = "Success", data = returnData });
        //}

        //public static JsonResult NotFound()
        //{
        //    return new JsonResult(new { status = "NotFound" });
        //}

        //public static JsonResult NotFound(object returnData)
        //{
        //    return new JsonResult(new { status = "NotFound", data = returnData });
        //}

        //public static JsonResult Error()
        //{
        //    return new JsonResult(new { status = "Error" });
        //}

        //public static JsonResult Error(object returnData)
        //{
        //    return new JsonResult(new { status = "Error", data = returnData });
        //}

        //public static JsonResult UnAuthorized()
        //{
        //    return new JsonResult(new { status = "UnAuthorized" });
        //}
        //public static JsonResult UnAuthorized(object returnData)
        //{
        //    return new JsonResult(new { status = "UnAuthorized", data = returnData });
        //}

        //public static JsonResult AccessDenied()
        //{
        //    return new JsonResult(new { status = "AccessDenied" });
        //}
        //public static JsonResult AccessDenied(object returnData)
        //{
        //    return new JsonResult(new { status = "AccessDenied", data = returnData });
        //}
    }
}
