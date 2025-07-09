using System;
using System.Collections.Generic;
using System.Text;
using DataModelLib;
using System.Diagnostics.CodeAnalysis;
using BlueprintLib.Attributes;

namespace LibraryExample
{
	[Blueprint("TableModel"), Table]
	public class Personn
	{
		[Column,PrimaryKey]
		public byte PersonnID { get; set; }

		[Column(DisplayName ="First name")]
		public string FirstName { get; set; }
		
		[Column]
		public string LastName { get; set; }
		
		[Column]
		public byte Age { get; set; }

		[Column, ForeignKey("DeliveryAddress", "DeliveredPeople", nameof(Address), nameof(Address.AddressID),CascadeTriggers.Delete)]
		public byte DeliveryAddressID { get; set; }

		[Column, ForeignKey("BillingAddress", "BilledPeople", nameof(Address), nameof(Address.AddressID), CascadeTriggers.Update)]
		public byte? BillingAddressID { get; set; }

		[Column, ForeignKey("PreferedPet", "Owners", nameof(Pet), nameof(Pet.PetID), CascadeTriggers.Update)]
		public byte PetID { get; set; }


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
