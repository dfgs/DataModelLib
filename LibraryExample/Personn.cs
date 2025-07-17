using System;
using System.Collections.Generic;
using System.Text;
using DataModelLib;
using System.Diagnostics.CodeAnalysis;
using BlueprintLib.Attributes;

namespace LibraryExample
{
	[DTO("Personn"), Blueprint("DTO"), Blueprint("TableModel"), Table]
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
