namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    public class DataType
    {
        public bool IsVoid { get; set; }

        public DoshikExternalApiType ExternalType { get; set; }

        //public override bool Equals(object obj)
        //{
        //    if (obj is DataType objDataType)
        //    {
        //        if (IsVoid && objDataType.IsVoid)
        //            return true;

        //        return ExternalType.ExternalName == objDataType.ExternalType.ExternalName;

        //    }
        //    else
        //        return false;
        //}
    }
}
