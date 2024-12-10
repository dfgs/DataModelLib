using DataModelLib.Common.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelLib.Common.SourceGenerator
{
	public interface ISourceGenerator<T>
		where T:SchemaObject
	{
		string GenerateSource(T SchemaObject);
	}
}
