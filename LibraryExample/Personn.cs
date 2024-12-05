using System;
using System.Collections.Generic;
using System.Text;
using DataModelGenerator;
using System.Diagnostics.CodeAnalysis;

namespace LibraryExample
{
	[Table]
	public class Personn
	{
		[Column,PrimaryKey]
		public byte PersonnID { get; set; }

		[Column]
		public string FirstName { get; set; }
		
		[Column]
		public string LastName { get; set; }
		
		[Column]
		public byte Age { get; set; }

		[Column, ForeignKey("DeliveryAddress", "DeliveredPeople", "Address", "AddressID",CascadeTriggers.Delete)]
		public byte DeliveryAddressID { get; set; }

		[Column, ForeignKey("BillingAddress", "BilledPeople", "Address", "AddressID", CascadeTriggers.Update)]
		public byte? BillingAddressID { get; set; }

		[Column, ForeignKey("PreferedPet", "Owners", "Pet", "PetID", CascadeTriggers.Update)]
		public byte PetID { get; set; }


		public Personn(byte PersonnID, string FirstName, string LastName, byte Age)
		{
			this.PersonnID = PersonnID; this.FirstName = FirstName;	this.LastName = LastName;this.Age = Age;
		}

	}
}
