// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "Why store it as a Uri, when it just gets transformed back to a string.", Scope = "member", Target = "~P:SelecTunes.Backend.Helper.AuthHelper.RedirectUrl")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.AppSettings.RedirectUri")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.ArtistItem.Uri")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.TrackItem.Uri")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.TrackItem.Artists")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.Tracks.Items")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.Album.Artists")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Helper.PlaybackHelper.Devices.Ope")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.Album.Images")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.Party.SongQueue")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.Party.PartyMembers")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.Party.KickedMembers")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.ArtistItem.Genres")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.Artists.Items")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.SongSearchIngestion.ArtistItem.Images")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:SelecTunes.Backend.Models.OneOff.Devices.Ope")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:SelecTunes.Backend.Controllers.UtilController.Version~Microsoft.AspNetCore.Mvc.ActionResult{System.String}")]
