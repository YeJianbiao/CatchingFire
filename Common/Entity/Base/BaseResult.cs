

namespace Entity.Base
{
    public class BaseResult<T> where T:class 
    {

        public BaseResult() { }

        public BaseResult(T data, bool isSuccess, string message, object obj)
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
            Obj = obj;
        } 

        public T Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Obj { get; set; }

    }
}
