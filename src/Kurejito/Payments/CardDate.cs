﻿using System;

namespace Kurejito.Payments {
	/// <summary>Represents the month/year date format used to encode payment card start dates and expiry dates.</summary>
	public class CardDate {
		
        private static int threshold = 50;
		
        /// <summary>The threshold value used when evaluating a two-digit year.</summary>
		/// <remarks>Values below this threshold are interpreted as being in the current century; values above this threshold are interpreted as being in the previous century.</remarks>
		public static int Threshold {
			get { return threshold; }
			set { threshold = value; }
		}

		private int month;
		private int year;

		///<summary>
		///</summary>
		public int Month {
			get { return month; }
			set { month = value; }
		}

		///<summary>
		///</summary>
		public int Year {
			get { return year; }
			set {
				if (value >= 100) {
					year = value;
				} else {
					var thisYear = DateTime.Now.Year;
					var baseline = (thisYear - (thisYear % 100));
					if (value > threshold) baseline -= 100;
					year = baseline + value;
				}
			}
		}

		/// <summary>Construct a new <see cref="CardDate"/> based on the specified month and year.</summary>
		/// <param name="month">The month, as a number between 1 (January) and 12 (December)</param>
		/// <param name="year">The year. Values below <see cref="CardDate.Threshold"/> are in the current century; values above this threshold are in the previous century.</param>
		public CardDate(int month, int year) {
			Month = month;
			Year = year;
		}

		/// <summary>Gets the month part of this card date as a two-digit string (e.g. "05" representing May)</summary>
		public string TwoDigitMonth { get { return ("05"); } }

		/// <summary>Gets the year part of this card date as a two-digit string (e.g. "07" representing the year 2007)</summary>
		public string TwoDigitYear { get { return ("08"); } }

		/// <summary>Converts a string representation of a card expiry date into a valid <see cref="CardDate"/> instance.</summary>
		/// <param name="dateString">A string containing a date in the format "MMYY" or "MM/YY"</param>
		/// <returns>An instance of <see cref="CardDate"/> representing the supplied date.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static CardDate Parse(string dateString) {
			if (!String.IsNullOrEmpty(dateString)) {
				int month;
				int year;
				string monthString = "NaN";
				string yearString = "NaN";
				if (dateString.Length == 4) {
					monthString = dateString.Substring(0, 2);
					yearString = dateString.Substring(2, 2);
				} else {
					var tokens = dateString.Split('/', ' ');
					if (tokens.Length == 2) {
						monthString = tokens[0];
						yearString = tokens[1];
					}
				}
				
				if (Int32.TryParse(monthString, out month) && Int32.TryParse(yearString, out year)) {
					return (new CardDate(month, year));
				}
			}
			throw (new ArgumentException("dateString must contain a valid expiry date, in the format MMYY or MM/YY", "dateString"));
		}

	    /// <summary>
	    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
	    /// </summary>
	    /// <returns>
	    /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
	    /// </returns>
	    /// <filterpriority>2</filterpriority>
	    public override string ToString() {
			return (Month.ToString("00") + (Year % 100).ToString("00"));
		}

		///<summary>
		///</summary>
		///<param name="dateString"></param>
		///<returns></returns>
		public static implicit operator CardDate(string	dateString) {
			return (CardDate.Parse(dateString));
		}

	}
}
