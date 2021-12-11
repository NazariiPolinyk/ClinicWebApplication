using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClinicWebApplication.DataLayer.Models;
using System.Text.RegularExpressions;

namespace ClinicWebApplication.BusinessLayer.Services.InputValidationService
{
    public static class InputValidation
    {
        private static readonly Regex NamePattern = new Regex(@"([А-ЯҐЄІЇ][а-яґєії][A-Z][a-z]+[\-\s]?){3,}");
        private static readonly Regex PhonePattern = new Regex(@"\d{10}");
        private static readonly Regex EmailPattern = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        private const int MinPasswordLength = 8, MaxPasswordLength = 30, MaxNameLength = 79, MaxPhoneNumberLength = 10, MinBirthDateYear = 1900, MaxExpierenceYear = 65;
        private const int MaxCategoryTextLength = 30, MaxDescriptionTextLength = 500, MaxFeedbackTextLength = 200, MaxDiagnosisTextLength = 200;
        public static (bool result, string error) ValidatePatient(Patient patient)
        {
            if(string.IsNullOrWhiteSpace(patient.Name) 
                || patient.Name.Length > MaxNameLength
                || NamePattern.IsMatch(patient.Name))
            {
                return (false, "Name is not valid.");
            }
            if(string.IsNullOrWhiteSpace(patient.Phone)
                || patient.Phone.Length > MaxPhoneNumberLength
                || PhonePattern.IsMatch(patient.Phone))
            {
                return (false, "Phone number is not valid.");
            }
            if(patient.BirthDate.Year < MinBirthDateYear
                || patient.BirthDate > DateTime.Now)
            {
                return (false, "Birth date is not valid.");
            }
            if(string.IsNullOrWhiteSpace(patient.Email)
                || EmailPattern.IsMatch(patient.Email))
            {
                return (false, "Email address is not valid.");
            }
            if(string.IsNullOrWhiteSpace(patient.Password)
                || patient.Password.Length < MinPasswordLength
                || patient.Password.Length > MaxPasswordLength)
            {
                return (false, "Password is not valid.");
            }
            return (true, string.Empty);
        }
        public static (bool result, string error) ValidateDoctor(Doctor doctor)
        {
            if(string.IsNullOrWhiteSpace(doctor.Name)
                || doctor.Name.Length > MaxNameLength
                || NamePattern.IsMatch(doctor.Name))
            {
                return (false, "Name is not valid.");
            }
            if(doctor.Experience < 0
                || doctor.Experience > MaxExpierenceYear)
            {
                return (false, "Years of experience is not valid.");
            }
            if(string.IsNullOrWhiteSpace(doctor.Category)
                || doctor.Category.Length > MaxCategoryTextLength)
            {
                return (false, "Category is not valid.");
            }
            if(string.IsNullOrWhiteSpace(doctor.Description)
                || doctor.Description.Length > MaxDescriptionTextLength)
            {
                return (false, "Description is empty or too long.");
            }
            if (string.IsNullOrWhiteSpace(doctor.Email)
                || EmailPattern.IsMatch(doctor.Email))
            {
                return (false, "Email address is not valid.");
            }
            if (string.IsNullOrWhiteSpace(doctor.Password)
                || doctor.Password.Length < MinPasswordLength
                || doctor.Password.Length > MaxPasswordLength)
            {
                return (false, "Password is not valid.");
            }
            return (true, string.Empty);
        }
        public static (bool result, string error) ValidateAppoinment(Appoinment appoinment)
        {
            if (string.IsNullOrWhiteSpace(appoinment.Description)
               || appoinment.Description.Length > MaxDescriptionTextLength)
            {
                return (false, "Description is empty or too long.");
            }
            return (true, string.Empty);
        }
        public static (bool result, string error) ValidateFeedback(Feedback feedback)
        {
            if(string.IsNullOrWhiteSpace(feedback.FeedbackText)
                || feedback.FeedbackText.Length > MaxFeedbackTextLength)
            {
                return (false, "Feedback text is empty or too long.");
            }
            return (true, string.Empty);
        }
        public static (bool result, string error) ValidateMedicalCardRecord(MedicalCardRecord medicalCardRecord)
        {
            if(string.IsNullOrWhiteSpace(medicalCardRecord.Diagnosis)
                || medicalCardRecord.Diagnosis.Length > MaxDiagnosisTextLength)
            {
                return (false, "Diagnosis is empty or too long.");
            }
            return (true, string.Empty);
        }
    }
}
