using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

[assembly: UdonProgramSourceNewMenu(typeof(UdonTestLangProgramAsset), "Udon Test lang Program Asset")]

[CreateAssetMenu(menuName = "VRChat/Udon/Udon Test lang Program Asset", fileName = "New Udon Test lang Program Asset")]
public class UdonTestLangProgramAsset : UdonAssemblyProgramAsset
{
    protected override void DoRefreshProgramActions()
    {
        var values = new Dictionary<string, (object value, Type type)>()
        {
            {
                "axis_0",
                ( value: new Vector3(0, 0, 1), type: typeof(Vector3) )
            },

            {
                "angle_0",
                ( value: (Single)1, type: typeof(Single) )
            },
        };

        udonAssembly = @"
.data_start

    
    instance_0: %UnityEngineTransform, this
    axis_0: %UnityEngineVector3, null
    angle_0: %SystemSingle, null

.data_end

.code_start

    .export _update
    
    _update:
    
        PUSH, instance_0
        PUSH, axis_0
        PUSH, angle_0
        EXTERN, ""UnityEngineTransform.__Rotate__UnityEngineVector3_SystemSingle__SystemVoid""
        JUMP, 0xFFFFFF

.code_end
        ";

        base.DoRefreshProgramActions();

        ApplyDefaultValuesToHeap(values);
    }

    protected void ApplyDefaultValuesToHeap(Dictionary<string, (object value, Type type)> heapDefaultValues)
    {
        IUdonSymbolTable symbolTable = program?.SymbolTable;
        IUdonHeap heap = program?.Heap;
        if (symbolTable == null || heap == null)
        {
            return;
        }

        foreach (KeyValuePair<string, (object value, Type type)> defaultValue in heapDefaultValues)
        {
            if (!symbolTable.HasAddressForSymbol(defaultValue.Key))
            {
                continue;
            }

            uint symbolAddress = symbolTable.GetAddressFromSymbol(defaultValue.Key);
            (object value, Type declaredType) = defaultValue.Value;
            if (typeof(UnityEngine.Object).IsAssignableFrom(declaredType))
            {
                if (value != null && !declaredType.IsInstanceOfType(value))
                {
                    heap.SetHeapVariable(symbolAddress, null, declaredType);
                    continue;
                }
                if ((UnityEngine.Object)value == null)
                {
                    heap.SetHeapVariable(symbolAddress, null, declaredType);
                    continue;
                }
            }

            if (value != null)
            {
                if (!declaredType.IsInstanceOfType(value))
                {
                    value = declaredType.IsValueType ? Activator.CreateInstance(declaredType) : null;
                }
            }

            heap.SetHeapVariable(symbolAddress, value, declaredType);
        }
    }
}