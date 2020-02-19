using System.Collections.Generic;

namespace Doshik
{
    /// <summary>
    /// Описание входного апи (Типы и методы, которые можно использовать в коде дошика)
    /// </summary>
    public class DoshikExternalApi
    {
        /// <summary>
        /// Версия структуры данных.
        /// Если меняется что-либо, связанное со структурой данных в АПИ, либо они по другому воспринимаются, то нужно перестраивать кэш АПИ
        /// </summary>
        public const string dataStructuresVersion = "1";

        public string DataStructuresVersion => dataStructuresVersion;

        /// <summary>
        /// Совокупная версия Udon SDK + VRChat SDK, задается при формировании апи на основе внешних библиотек (по сути это версия этих внешних библиотек)
        /// </summary>
        public string ExternalVersion { get; set; }

        public List<DoshikExternalApiType> Types { get; set; }

        public List<DoshikExternalApiEvent> Events { get; set; }
    }

    public class DoshikExternalApiEvent
    {
        /// <summary>
        /// Имя события так, как оно будет указываться в udon assembly. Используется для генерации итоговой udon assembly.
        /// </summary>
        public string ExternalName { get; set; }

        /// <summary>
        /// Имя события так, как оно будет указываться в коде дошика. Используется для внутренней работы компилятора и референса из кода дошика
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Входные параметры (ключ - имя параметра, значение - тип параметра)
        /// В графе это выходные параметры. То есть это выходные параметры события, но входные параметры обработчика события (по этому и названо входными параметрами).
        /// </summary>
        public List<DoshikExternalApiMethodParameter> InParameters { get; set; }
    }

    /// <summary>
    /// Тип данных
    /// </summary>
    public class DoshikExternalApiType
    {
        /// <summary>
        /// Имя типа так, как оно будет указываться в udon assembly. Используется для генерации итоговой udon assembly.
        /// </summary>
        public string ExternalName { get; set; }

        /// <summary>
        /// Имя типа так, как оно будет указываться в коде дошика. Используется для внутренней работы компилятора и референса из кода 
        /// Каждый элемент массива это идентификатор, между которыми может быть разделение. Последний идентификатор это имя типа, предыдущие - части namespace
        /// Также возможно далее появятся generic type arguments, тогда я поменяю эту структуру
        /// </summary>
        public string[] FullyQualifiedCodeName { get; set; }

        // Флаги, обозначающие, был ли тип определен в определенной категории нод. Если ни в одной - значит тип взялся
        // из параметров метода (либо типа класса, к которому принадлежит метод) из параметров события либо любых других параметров

        /// <summary>
        /// Значит тип был определен как Const_ нода
        /// </summary>
        public bool DeclaredAsConstNode { get; set; }

        /// <summary>
        /// Значит тип был определен как Type_ нода
        /// </summary>
        public bool DeclaredAsTypeNode { get; set; }

        /// <summary>
        /// Значит тип был определен как Variable_ нода
        /// </summary>
        public bool DeclaredAsVariableNode { get; set; }

        /// <summary>
        /// Используется для констант типа System.Type где нужно дать ссылку на runtime тип данных
        /// </summary>
        public string DotnetTypeString { get; set; }

        /// <summary>
        /// Все методы, принадлежащие этому типу
        /// </summary>
        public List<DoshikExternalApiTypeMethod> Methods { get; set; }
    }

    /// <summary>
    /// Метод, связанный с определенным типом данных
    /// </summary>
    public class DoshikExternalApiTypeMethod
    {
        /// <summary>
        /// Тип, к которому принадлежит метод
        /// </summary>
        public DoshikExternalApiType Type { get; set; }

        /// <summary>
        /// Имя метода так, как оно будет указываться в udon assembly. Используется для генерации итоговой udon assembly.
        /// Это только имя метода отдельно, полная сигнатура есть в перегрузках
        /// </summary>
        public string ExternalName { get; set; }

        /// <summary>
        /// Имя метода так, как оно будет указываться в коде дошика. Используется для внутренней работы компилятора и референса из кода дошика
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Все перегрузки методов с таким именем (могут быть как статическими, так и instance)
        /// </summary>
        public List<DoshikExternalApiTypeMethodOverload> Overloads { get; set; }
    }

    /// <summary>
    /// Перегрузка метода
    /// </summary>
    public class DoshikExternalApiTypeMethodOverload
    {
        /// <summary>
        /// Метод, которому принадлежит перегрузка
        /// </summary>
        public DoshikExternalApiTypeMethod Method { get; set; }

        /// <summary>
        /// Имя типа к которому принадлежит перегрузка метода. Это по сути хак, т.к. это сделано для случаев когда имя типа тут будет
        /// интерфейсом udonbehaviour-а а реальный тип будет экземпляром udonbehaviour-а. Вроде как эта разница есть только в этих случаях.
        /// По хорошему надо реализовывать систему где нужно указывать методы на интерфейсы отдельно а при вызове метода из типа нужно искать реализацию
        /// не только в самом типе но и среди интерфейсов, которые этот тип реализует(либо базовых классов). Т.к. это довольно геморно - пока сделал такой костыль.
        /// Это имя типа используется для генерации полного названия extern call, а имя которое находится в тип используется для объялвения переменных
        /// </summary>
        public string ExternalTypeName { get; set; }

        /// <summary>
        /// Часть полной сигнатуры метода, нужной для udon assembly вызовов. Включает в себя входные и выходные параметры
        /// Заметка: Нельзя однозначно для всех методов узнать это имя имея только параметры, т.к. параметры, указанные в сигнатуре могут отличаться порядком объялвения от тех что указаны входные/выходные
        /// В сигнатуре могут быть Ref параметры (что значит ref/out) которые соотвествуют выходным, но они могут стоять посередине сигнатуры и их может быть несколько, так что точно определить сигнатуру нельзя,
        /// но она нужна для идентификации перегрузки метода в assembly
        /// </summary>
        public string ExternalName { get; set; }

        /// <summary>
        /// Входные параметры (ключ - имя параметра, значение - тип параметра)
        /// Имена есть не всегда. Иногда "", иногда null
        /// </summary>
        public List<DoshikExternalApiMethodParameter> InParameters { get; set; }

        /// <summary>
        /// Основной выходной параметр. Может быть null, в случае если это void
        /// </summary>
        public DoshikExternalApiType OutParameterType { get; set; }

        /// <summary>
        /// Дополнительные выходные параметры (ref/out)
        /// </summary>
        public List<DoshikExternalApiMethodParameter> ExtraOutParameters { get; set; }

        /// <summary>
        /// Если не static, значит кроме указанных параметров, первым параметром идет еще "instance" параметр типа Method.Type
        /// </summary>
        public bool IsStatic { get; set; }
    }

    public class DoshikExternalApiMethodParameter
    {
        public string Name { get; set; }
        public DoshikExternalApiType Type { get; set; }
    }
}
