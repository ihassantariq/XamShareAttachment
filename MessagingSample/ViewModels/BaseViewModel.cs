﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MessagingSample.ViewModels
{
	/// <summary>
	/// Base class for all view models
	/// - Implements INotifyPropertyChanged for WinRT
	/// - Implements some basic validation logic
	/// - Implements some IsBusy logic
	/// </summary>
	public class BaseViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Event for property changes
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Event for when IsBusy changes
		/// </summary>
		public event EventHandler IsBusyChanged;

		/// <summary>
		/// Event for when IsValid changes
		/// </summary>
		public event EventHandler IsValidChanged;

		readonly List<string> errors = new List<string>();
		bool isBusy = false;

		/// <summary>
		/// Default constructor
		/// </summary>
		public BaseViewModel()
		{
			//Make sure validation is performed on startup
			Validate();
		}

		/// <summary>
		/// Returns true if the current state of the ViewModel is valid
		/// </summary>
		public bool IsValid
		{
			get { return errors.Count == 0; }
		}

		/// <summary>
		/// A list of errors if IsValid is false
		/// </summary>
		/// <returns>
		/// Return List of strings containing errors
		/// </returns>
		protected List<string> Errors
		{
			get { return errors; }
		}

		/// <summary>
		/// An aggregated error message
		/// </summary>
		/// <returns>
		/// Returns error string
		/// </returns>
		public virtual string Error
		{
			get
			{
				return errors.Aggregate(new StringBuilder(), (b, s) => b.AppendLine(s)).ToString().Trim();
			}
		}

		/// <summary>
		/// Protected method for validating the ViewModel
		/// - Fires PropertyChanged for IsValid and Errors
		/// </summary>
		protected virtual void Validate()
		{
			OnPropertyChanged("IsValid");
			OnPropertyChanged("Errors");

			var method = IsValidChanged;
			if (method != null)
				method(this, EventArgs.Empty);
		}

		/// <summary>
		/// Other viewmodels should call this when overriding Validate, to validate each property
		/// </summary>
		/// <param name="validate">Func to determine if a value is valid</param>
		/// <param name="error">The error message to use if not valid</param>
		protected virtual void ValidateProperty(Func<bool> validate, string error)
		{
			if (validate())
			{
				if (!Errors.Contains(error))
					Errors.Add(error);
			}
			else
			{
				Errors.Remove(error);
			}
		}

		/// <summary>
		/// Value indicating if a spinner should be shown
		/// </summary>
		/// <returns>
		/// Returns boolean specifying if view model is busy
		/// </returns>
		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				if (isBusy != value)
				{
					isBusy = value;

					OnPropertyChanged("IsBusy");
					OnIsBusyChanged();
				}
			}
		}

		/// <summary>
		/// Value indicating if a spinner should be shown
		/// </summary>
		/// <returns>
		/// Returns boolean specifying if view model is busy
		/// </returns>
		public bool IsNotBusy
		{
			get { return !isBusy; }
		}

		/// <summary>
		/// Other viewmodels can override this if something should be done when busy
		/// </summary>
		protected virtual void OnIsBusyChanged()
		{
			var ev = IsBusyChanged;
			if (ev != null)
			{
				ev(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Property changed event base implementation
		/// </summary>
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var ev = PropertyChanged;
			if (ev != null)
			{
				ev(this, new PropertyChangedEventArgs(propertyName));
			}
		}


	}
}