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
        public static (bool result, string error) ValidatePatient(Patient patient)
        {
            if(string.IsNullOrWhiteSpace(patient.Name) 
                || patient.Name.Length > 49
                || !new Regex(@"([А-ЯҐЄІЇ][а-яґєії][A-Z][a-z]+[\-\s]?){3,}").IsMatch(patient.Name))
            {
                return (false, "Name is not valid.");
            }
            if(string.IsNullOrWhiteSpace(patient.Phone)
                || patient.Phone.Length > 10
                || !new Regex(@"\d{10}").IsMatch(patient.Phone))
            {
                return (false, "Phone number is not valid.");
            }
            if(patient.BirthDate.Year < 1900
                || patient.BirthDate > DateTime.Now)
            {
                return (false, "Birth date is not valid.");
            }
            if(string.IsNullOrWhiteSpace(patient.Email)
                || !new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").IsMatch(patient.Email))
            {
                return (false, "Email address is not valid.");
            }
            if(string.IsNullOrWhiteSpace(patient.Password)
                || patient.Password.Length < 8
                || patient.Password.Length > 30)
            {
                return (false, "Password is not valid.");
            }
            return (true, string.Empty);
        }
        public static (bool result, string error) ValidateDoctor(Doctor doctor)
        {
            if(string.IsNullOrWhiteSpace(doctor.Name)
                || doctor.Name.Length > 49
                || !new Regex(@"([А-ЯҐЄІЇ][а-яґєії][A-Z][a-z]+[\-\s]?){3,}").IsMatch(doctor.Name))
            {
                return (false, "Name is not valid.");
            }
            if(doctor.Experience < 0
                || doctor.Experience > 65)
            {
                return (false, "Years of experience is not valid.");
            }
            if(string.IsNullOrWhiteSpace(doctor.Category)
                || doctor.Category.Length > 30)
            {
                return (false, "Category is not valid.");
            }
            if(string.IsNullOrWhiteSpace(doctor.Description)
                || doctor.Description.Length > 500)
            {
                return (false, "Description is empty or too long.");
            }
            if (string.IsNullOrWhiteSpace(doctor.Email)
                || !new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").IsMatch(doctor.Email))
            {
                return (false, "Email address is not valid.");
            }
            if (string.IsNullOrWhiteSpace(doctor.Password)
                || doctor.Password.Length < 8
                || doctor.Password.Length > 30)
            {
                return (false, "Password is not valid.");
            }
            return (true, string.Empty);
        }
        public static (bool result, string error) ValidateAppoinment(Appoinment appoinment)
        {
            if (string.IsNullOrWhiteSpace(appoinment.Description)
               || appoinment.Description.Length > 500)
            {
                return (false, "Description is empty or too long.");
            }
            return (true, string.Empty);
        }
        public static (bool result, string error) ValidateFeedback(Feedback feedback)
        {
            if(string.IsNullOrWhiteSpace(feedback.FeedbackText)
                || feedback.FeedbackText.Length > 200)
            {
                return (false, "Feedback text is empty or too long.");
            }
            return (true, string.Empty);
        }
        public static (bool result, string error) ValidateMedicalCardRecord(MedicalCardRecord medicalCardRecord)
        {
            if(string.IsNullOrWhiteSpace(medicalCardRecord.Diagnosis)
                || medicalCardRecord.Diagnosis.Length > 200)
            {
                return (false, "Diagnosis is empty or too long.");
            }
            return (true, string.Empty);
        }
    }
}
