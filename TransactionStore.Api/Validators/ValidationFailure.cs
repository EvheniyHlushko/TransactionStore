using System.Runtime.Serialization;

namespace TransactionStore.Api.Validators
{
    [DataContract]
    public class ValidationFailure
    {
        public string PropertyName { get; set; } = "";


        [DataMember(Name = "message")] public string ErrorMessage { get; set; } = "";

        public string? RawRecord { get; set; }
    }
}