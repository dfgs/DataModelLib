using System;
using System.Collections.Generic;
using System.Text;
using DataLib;
using System.Diagnostics.CodeAnalysis;
using BlueprintLib.Attributes;

namespace LibraryExample.Tables
{
	[Table, DTO("Pet"),Blueprint("Table")]
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
