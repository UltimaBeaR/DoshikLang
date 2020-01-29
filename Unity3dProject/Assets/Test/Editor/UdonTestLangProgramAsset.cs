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
                "constant_0",
                ( value: (System.Single)18.5, type: typeof(System.Single) )
            },

            {
                "constant_1",
                ( value: (System.Single)5, type: typeof(System.Single) )
            },

                        {
                "constant_2",
                ( value: (System.Single)2, type: typeof(System.Single) )
            },
        };

        udonAssembly = @"
# compiled with UAssemblyBuilder

.data_start


    constant_0: %SystemSingle, null
    constant_1: %SystemSingle, null
    constant_2: %SystemSingle, null
    global_private_a_0: %SystemSingle, null
    local_event_Update_result_0: %SystemSingle, null
    temp_0: %SystemSingle, null
    temp_1: %SystemSingle, null
    temp_2: %SystemObject, null

.data_end

.code_start

    .export _start
    .export _update

    _start:
        PUSH, constant_0  # 0x000000
        PUSH, global_private_a_0  # 0x000005
        COPY  # 0x00000A
        JUMP, 0xFFFFFF  # 0x00000B

    _update:
        PUSH, global_private_a_0  # 0x000010
        PUSH, constant_1  # 0x000015
        PUSH, temp_0  # 0x00001A
        EXTERN, ""SystemSingle.__op_Addition__SystemSingle_SystemSingle__SystemSingle""  # 0x00001F
        PUSH, temp_0  # 0x000024
        PUSH, constant_2  # 0x000029
        PUSH, temp_1  # 0x00002E
        EXTERN, ""SystemSingle.__op_Multiplication__SystemSingle_SystemSingle__SystemSingle""  # 0x000033
        PUSH, temp_1  # 0x000038
        PUSH, local_event_Update_result_0  # 0x00003D
        COPY  # 0x000042
        PUSH, local_event_Update_result_0  # 0x000043
        PUSH, temp_2  # 0x000048
        COPY  # 0x00004D
        PUSH, temp_2  # 0x00004E
        EXTERN, ""UnityEngineDebug.__Log__SystemObject__SystemVoid""  # 0x000053
        JUMP, 0xFFFFFF  # 0x000058

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