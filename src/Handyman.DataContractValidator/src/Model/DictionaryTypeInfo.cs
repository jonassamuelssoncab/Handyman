﻿using Handyman.DataContractValidator.CodeGen;
using Handyman.DataContractValidator.CodeGen.DataContracts;
using Handyman.DataContractValidator.Validation;

namespace Handyman.DataContractValidator.Model
{
    internal class DictionaryTypeInfo : TypeInfo
    {
        public TypeInfo Key { get; set; }
        public TypeInfo Value { get; set; }
        public override string TypeName => "dictionary";

        public override ITypeInfoValidator GetValidator()
        {
            return new DictionaryValidator();
        }

        public override ISyntaxNode GetDataContractSyntaxNode()
        {
            return DictionarySyntaxNode.Create(this);
        }
    }
}