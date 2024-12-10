using DataModelLib.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.SourceGenerator
{
	public abstract class SourceGenerator<T>: ISourceGenerator<T>
		where T:SchemaObject
	{
		public abstract string GenerateSource(T SchemaObject);
	}
}
