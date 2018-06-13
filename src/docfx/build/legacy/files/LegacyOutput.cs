// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Docs.Build
{
    internal static class LegacyOutput
    {
        public static void Convert(Docset docset, Context context, List<(LegacyManifestItem manifestItem, Document document)> files)
        {
            Parallel.ForEach(files, file =>
            {
                var document = file.document;
                var manifestItem = file.manifestItem;
                switch (document.ContentType)
                {
                    case ContentType.TableOfContents:
                        LegacyTableOfContents.Convert(docset, context, document, manifestItem.Output);
                        break;
                    case ContentType.Markdown:
                        LegacyMarkdown.Convert(docset, context, document, manifestItem.Output);
                        break;
                    case ContentType.Asset:
                        File.Move(Path.Combine(docset.Config.Output.Path, document.OutputPath), Path.Combine(docset.Config.Output.Path, manifestItem.Output.ResourceOutput.ToLegacyOutputPath(docset)));
                        context.WriteJson(new { }, manifestItem.Output.MetadataOutput.ToLegacyOutputPath(docset)); // todo: art metadata
                        break;
                }
            });
        }
    }
}