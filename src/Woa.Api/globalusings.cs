global using System;
global using System.Buffers;
global using System.Collections.Concurrent;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Net.Http;
global using System.Runtime.Serialization;
global using System.ServiceModel;
global using System.Text;
global using System.Threading.Tasks;




global using Microsoft.AspNetCore;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Primitives;

global using Faslinq;

global using Markdig;
global using Markdig.SyntaxHighlighting;

global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;

#if WOA_API
global using Woa.Api.Common;
global using Woa.Api.Contracts;
global using Woa.Api.DataModels;
global using Woa.Api.Startup;
global using Woa.Api.Services;
#endif

global using Woa.MarkdownServer.Common;
global using Woa.MarkdownServer;
