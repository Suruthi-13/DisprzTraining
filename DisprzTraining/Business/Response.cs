using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public  class NotFoundResponse
    {
        public  string LanguageCode {get;set;} = "en";
        public  string ErrorMessage {get;set;} ="The searched context does not have anything";
        public  string ErrorCode {get;set;} ="DZ_Common_003";
       
    }
    public class InvalidTimeIntervalresponse
    {
        public string LanguageCode {get;set;}="en";
        public string ErrorMessage {get;set;} ="Check the Time Interval";
        public string ErrorCode {get;set;} ="DZ_AuthN-002";

    }
    public  class NotAllowedPastResponse
    {
        public  string LanguageCode {get;set;} = "en";
        public  string ErrorMessage {get;set;} ="Meeting does not allowed for past";
        public  string ErrorCode {get;set;} ="DZ_AuthN_001";
       
    }
    
    public  class ConflictResponse
    {
        public  string LanguageCode{get;set;}="en";
        public string ErrorMessage{get;set;}="Conflict occurs.Meeting was already excisted in that timing";
        public  string ErrorCode{get;set;}="DZ_409";
    }
    public class updatedConflictResponse
    {
       public  string LanguageCode{get;set;}="en";
        public string ErrorMessage{get;set;}="Conflict occurs,Meeting was already excisted in that timing";
        public  string ErrorCode{get;set;}="DZ_409"; 
    }
}