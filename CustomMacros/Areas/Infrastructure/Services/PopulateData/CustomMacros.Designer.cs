﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomMacros.Areas.Infrastructure.Services.PopulateData {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CustomMacros {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CustomMacros() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CustomMacros.Areas.Infrastructure.Services.PopulateData.CustomMacros", typeof(CustomMacros).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT CASE WHEN EXISTS (SELECT 1 FROM [DBO].[CMSMACRO]) 
        ///		  THEN CAST(1 AS BIT) 
        ///		  ELSE CAST(0 AS BIT)
        ///	   END.
        /// </summary>
        internal static string IsInitialized {
            get {
                return ResourceManager.GetString("IsInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SET NUMERIC_ROUNDABORT OFF
        ///SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
        ///
        ///DECLARE @pv binary(16)
        ///BEGIN TRANSACTION
        ///ALTER TABLE [dbo].[cmsLanguageText] DROP CONSTRAINT [FK_cmsLanguageText_cmsDictionary_id]
        ///ALTER TABLE [dbo].[cmsContentTypeAllowedContentType] DROP CONSTRAINT [FK_cmsContentTypeAllowedContentType_cmsContentType]
        ///ALTER TABLE [dbo].[cmsContentTypeAllowedContentType] DROP CONSTRAINT [FK_cmsContentTypeAllowedContentType_cms [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PopulateCustomMacroData {
            get {
                return ResourceManager.GetString("PopulateCustomMacroData", resourceCulture);
            }
        }
    }
}
