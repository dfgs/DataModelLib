using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DataModelLib.DataModels
{
	public abstract class DataModel
	{
		
		
		public DataModel()
		{
		}

		public abstract string GenerateDatabaseSource();
		public abstract string GenerateDatabaseConstructorSource();
		public abstract string GenerateDatabaseModelSource();
	}
}
