using DataLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BlueprintLib.Attributes;

namespace LibraryExample.Tables
{
	
	[Table, DTO("Address"), Blueprint("Table")]
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
