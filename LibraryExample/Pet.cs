using System;
using System.Collections.Generic;
using System.Text;
using DataModelLib;
using System.Diagnostics.CodeAnalysis;
using BlueprintLib.Attributes;

namespace LibraryExample
{
	[Table, DTO("Pet"),Blueprint("DTO")]
	public partial class Pet
	{
		
		public Pet(byte PetID, string Name)
		{
			this.PetID = PetID;this.Name = Name;
		}
		public override string ToString()
		{
			return $"{Name}";
		}


	}
}
