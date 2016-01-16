using System;
using Foundation;
using System.Collections.Generic;

namespace Nomads.Utilities.Extensions
{
	public class PredicateData
	{
		public string KeyString { get; set; }

		public string OperatorString { get; set; }

		public string ValueString { get; set; }

		public string OriginalString { get; private set; }

		public PredicateData (string predicateString)
		{
			OriginalString = predicateString;
		}
	}

	public static class NSPredicateExtensions
	{
	
		static List<string> operatorStrings = new List<string> { "==", ">=", ">", "<=", "<", "!=" };

		//		Account Type BEGINSWITH "Customer"
		//		Account Type BETWEEN "Customer"
		//		Account Type CONTAINS "Customer"
		//		Account Type ENDSWITH "Customer"
		//		Account Type == "Customer"
		//		Account Type > "Customer"
		//		Account Type >= "Customer"
		//		Account Type IN "Customer"
		//		Account Type < "Customer"
		//		Account Type <= "Customer"
		//		Account Type LIKE "Customer"
		//		Account Type MATCHES "Customer"
		//		Account Type != "Customer"

		public static string GetValueString (this NSPredicate predicate)
		{
			foreach (var operatorString in operatorStrings) {

				var index = predicate.PredicateFormat.LastIndexOf(operatorString, StringComparison.Ordinal);

				if (index > 0) {

					var val = predicate.PredicateFormat
						.Substring(index)?
						.Replace(operatorString, string.Empty)?
						.Replace(" \"", string.Empty)?
						.Replace("\"", string.Empty);
				
					if (!string.IsNullOrWhiteSpace(val)) return val;
				}
			}

			return null;
		}

		public static PredicateData GetPredicateData (this NSPredicate predicate)
		{
			var predicateString = predicate.PredicateFormat;

			var predicateData = new PredicateData (predicateString);

			foreach (var operatorString in operatorStrings) {

				var paddedOperatorString = string.Format($" {operatorString} ");

				var index = predicateString.LastIndexOf(paddedOperatorString, StringComparison.Ordinal);

				if (index > 0) {

					predicateData.KeyString = predicateString.Substring(0, index);

					predicateData.ValueString = predicateString
						.Substring(index + paddedOperatorString.Length)?
						.Replace("\"", string.Empty);

					predicateData.OperatorString = operatorString.Replace("==", "=");

					return predicateData;
				}
			}

			return predicateData;
		}

		//		public static NSPredicate GetNSPredicate (this PredicateData predicateData)
		//		{
		//
		//		}

		public static string ToSalesforceString (this NSPredicateOperatorType type)
		{
			switch (type) {
//			case NSPredicateOperatorType.BeginsWith:
//			case NSPredicateOperatorType.Between:
//			case NSPredicateOperatorType.Contains:
//			case NSPredicateOperatorType.CustomSelector:
//			case NSPredicateOperatorType.EndsWith:
//			case NSPredicateOperatorType.In:
//			case NSPredicateOperatorType.Like:
//			case NSPredicateOperatorType.Matches:
			case NSPredicateOperatorType.EqualTo:
				return "=";
			case NSPredicateOperatorType.GreaterThan:
				return ">";
			case NSPredicateOperatorType.GreaterThanOrEqualTo:
				return ">=";
			case NSPredicateOperatorType.LessThan:
				return "<";
			case NSPredicateOperatorType.LessThanOrEqualTo:
				return "<=";
			case NSPredicateOperatorType.NotEqualTo:
				return "!=";
			default:
				throw new NotSupportedException ();
			}
		}


		public static string ToSalesforceString (this NSCompoundPredicateType type)
		{
			switch (type) {
			case NSCompoundPredicateType.And:
				return " AND ";
			case NSCompoundPredicateType.Or:
				return " OR ";
			case NSCompoundPredicateType.Not:
				throw new NotSupportedException ("This probably should be supported");
			default:
				return string.Empty;
			}
		}

		public static NSPredicateOperatorType[] ToArray (this NSPredicateOperatorType type)
		{
			return new [] { type };
		}
	}
}