﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrotiNet;

namespace Nekoxy
{
    /// <summary>
    /// HTTPリクエストデータ。
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// リクエストライン、ヘッダ、ボディを元に初期化。
        /// </summary>
        /// <param name="requestLine">HTTPリクエストライン</param>
        /// <param name="headers">HTTPリクエストヘッダ</param>
        /// <param name="body">HTTPリクエストボディ</param>
        public HttpRequest(HttpRequestLine requestLine, HttpHeaders headers, byte[] body)
        {
            this.RequestLine = requestLine;
            this.Headers = headers;
            this.Body = body;
        }

        /// <summary>
        /// HTTPリクエストライン。
        /// </summary>
        public HttpRequestLine RequestLine { get; private set; }

        /// <summary>
        /// HTTPヘッダ。
        /// </summary>
        public HttpHeaders Headers { get; private set; }

        /// <summary>
        /// HTTPリクエストボディ。
        /// Transfer-Encoding: chunked なHTTPリクエストの RequestBody の読み取りは未対応。
        /// </summary>
        public byte[] Body { get; private set; }

        /// <summary>
        /// パスとクエリ。
        /// </summary>
        public string PathAndQuery
        {
            //RequestLine.URIはUpstreamProxyを設定した際FULLパスになる
            get
            {
                return this.RequestLine.URI.StartsWith("/")
                    ? this.RequestLine.URI
                    : new Uri(this.RequestLine.URI).PathAndQuery;
            }
        }

        /// <summary>
        /// リクエストの文字エンコーディング。
        /// content-typeヘッダに指定されたcharsetを元に生成される。
        /// 指定がない場合はUS-ASCII。
        /// </summary>
        public Encoding Charset
        {
            get { return this.Headers.GetEncoding(); }
        }

        /// <summary>
        /// HTTPリクエストボディを文字列で取得する。
        /// Transfer-Encoding: chunked なHTTPリクエストの RequestBody の読み取りは未対応。
        /// </summary>
        public string BodyAsString
        {
            get { return this.Body != null ? this.Charset.GetString(this.Body) : null; }
        }
    }
}