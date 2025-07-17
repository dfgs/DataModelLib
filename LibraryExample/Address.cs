using DataModelLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BlueprintLib.Attributes;

namespace LibraryExample
{
	
	[DTO("Address"), Blueprint("DTO"), Blueprint("TableModel"), Table]
	public partial class Address 
	{
		
		public Address(byte AddressID, string Street)
		{
			this.AddressID = AddressID; this.Street = Street;
		}
		public override string ToString()
		{
			return $"{Number} {Street}";
		}


	}
}
