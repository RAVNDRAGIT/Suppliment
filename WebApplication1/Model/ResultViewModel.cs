namespace Suppliment.API.Model
{
    public class ResultViewModel<T> where T : class
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; } 
        public List<T> list {get;set ;}
        
    }
}
