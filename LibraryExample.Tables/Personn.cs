using System;
using System.Collections.Generic;
using System.Text;
using DataLib;
using System.Diagnostics.CodeAnalysis;
using BlueprintLib.Attributes;

namespace LibraryExample.Tables
{
	[Table, DTO("Personn"), Blueprint("Table")]
	public partial class Personn
	{
		
		public Personn(byte PersonnID, string FirstName, string LastName, byte Age)
		{
			this.PersonnID = PersonnID; this.FirstName = FirstName;	this.LastName = LastName;this.Age = Age;
		}

		public override string ToString()
		{
			return $"{FirstName} {LastName}";
		}
	}
}
