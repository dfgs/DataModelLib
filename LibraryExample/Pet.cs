using System;
using System.Collections.Generic;
using System.Text;
using DataModelLib;
using System.Diagnostics.CodeAnalysis;
using BlueprintLib.Attributes;

namespace LibraryExample
{
	[Blueprint("TableModel.bp"), Table]
	public class Pet
	{
		[Column(DisplayName ="Pet ID"),PrimaryKey]
		public byte PetID { get; set; }

		[Column]
		public string Name { get; set; }
		
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
