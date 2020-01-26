namespace DoshikLangCompiler.Compilation.CodeRepresentation
{
    // !!! на данный момент нельзя сравнивать их по референсам, т.к. эти типы создаются каждый раз новые.
    // Можно сравнивать либо по внутренностям, либо сделать общую базу типов и обращаться каждый раз туда (делать GetOrCreate)
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
