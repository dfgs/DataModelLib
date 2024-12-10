using DataModelLib.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.SourceGenerator
{
	public interface ISourceGenerator<T>
		where T:SchemaObject
	{
		string GenerateSource(T SchemaObject);
	}
}
