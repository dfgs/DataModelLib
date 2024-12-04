using System;
using System.Collections.Generic;
using System.Text;
using DataModelGenerator;
using System.Diagnostics.CodeAnalysis;

namespace LibraryExample
{
	[Table("People")]
	public class Personn
	{
		[Column]
		public byte PersonnID { get; set; }

		[Column]
		public string FirstName { get; set; }
		
		[Column]
		public string LastName { get; set; }
		
		[Column]
		public byte Age { get; set; }
		
		[Column,ForeignKey("DeliveryAddress","Addresses","AddressID")]
		public byte DeliveryAddressID { get; set; }


		public Personn(string FirstName, string LastName, byte Age)
		{
			this.FirstName = FirstName;	this.LastName = LastName;this.Age = Age;
		}

	}
}
