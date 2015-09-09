
var REQ = 
{
	XMLHttpFactories: [
		function () { return new XMLHttpRequest() },
		function () { return new ActiveXObject("Msxml2.XMLHTTP") },
		function () { return new ActiveXObject("Msxml3.XMLHTTP") },
		function () { return new ActiveXObject("Microsoft.XMLHTTP") }
	]

	, createXMLHTTPObject: function ()
    {
		var xmlhttp = false;
		for (var i = 0; i < this.XMLHttpFactories.length; i++)
		{
			try
			{
				xmlhttp = this.XMLHttpFactories[i]();
			}
			catch (e)
			{
				continue;
			}
			break;
		} // Next i
		
		return xmlhttp;
	} // End Function createXMLHTTPObject
	
	, urlEncode: function (pd)
    {
		if (typeof pd == 'string' || pd instanceof String)
			return pd; // encodeURI(pd); // Might be stringified JSON

		var k, i = 0, str = "";;
		for (k in pd)
		{
			//if(i!=0)
			str += "&";

			str += encodeURIComponent(k) + "=" + encodeURIComponent(pd[k]);
			++i;
		}

		return str;
	}

    // REQ.webMethod("AJAX.aspx", "popDrop", cbSuccess, cbError, postData);
	, "webMethod": function (url, method, onSuccess, onError, onDone, postData, addParams)
	{
	    var url = url + "/" + method;
	    this.sendRequest(url
            , function (r) // onsSuccess
            {
                if (onSuccess)
		            onSuccess(r, addParams);
            }
            
            , function (r) // onError
            {
                if (onError)
                {
					var ex = null;
					
					try
					{
						ex = JSON.parse(r.responseText);
					}
					catch(e)
					{
					    // logMyErrors(e);
					}
                    
                    onError(r, ex);
                }
                else
                    alert('HTTP error ' + r.status);
            }

            , onDone // Always
		    , postData, "application/json; charset=utf-8", "POST"
        );
	}

	, "get": function (url, onSuccess, postData)
    {
	    this.sendRequest(url, onSuccess, null, null, postData, 'application/urlencode', "GET");
	}

	, "post": function (url, onSuccess, postData, contentType)
	{
	    this.sendRequest(url, onSuccess, null, null, postData, 'application/x-www-form-urlencoded', "POST");
	}

	, "postJSON": function (url, onSuccess, postData, contentType)
	{
	    this.sendRequest(url, onSuccess, null, null, postData, 'application/json; charset=UTF-8', "POST");
	}

	, "sendRequest": function (url, onSuccess, onError, onDone, postData, contentType, method)
    {
	    url += ((url.indexOf('?') === -1) ? "?" : "&") + "no_cache=" + (new Date()).getTime();

		if (postData)
		{
			if (!method)
				method = "POST";

			if (method.toUpperCase() === "GET")
				url += this.urlEncode(postData);
			else
			{
				if (!contentType)
				    contentType = 'application/x-www-form-urlencoded';

				if (contentType.indexOf("application/json") != -1)
				{
				    if (!(typeof postData == 'string' || postData instanceof String))
				        postData = JSON.stringify(postData);
				}

				if (contentType.indexOf("application/x-www-form-urlencoded") != -1)
				    postData = this.urlEncode(postData);
			}

		}
		else if (!method)
			method = "GET";
			
			
		var req = this.createXMLHTTPObject();
		if (!req) return;

		if (false)
		{
		    if (url.indexOf("popDrop") === -1)
		    { 
		        console.log("url: " + url);
		        console.log("method: " + method);
		        console.log("contentType: " + contentType);
			    console.log("data:");
			    if (method.toUpperCase() === "GET")
				    console.log(url);
			    else
			        console.log(postData);
		    }
		}

		req.open(method, url, true);
		// req.setRequestHeader('User-Agent', 'XMLHTTP/1.0');
		req.setRequestHeader('cache-control', 'no-cache');
		
		if (postData)
			req.setRequestHeader('Content-type', contentType);
			
		req.onreadystatechange = function ()
		{
			if (req.readyState != 4) return;
			
			if (!(req.status != 200 && req.status != 304 && req.status != 0))
			{
			    if (onSuccess)
			        onSuccess(req.responseText);
			}
			
			if (req.status != 200 && req.status != 304 && req.status != 0)
			{
			    if (onError)
			        onError(req);
			    else
			    {
			        alert('HTTP error ' + req.status);
			        // return;
			    }
			}
			
			if (onDone)
			    onDone(req);
		}
		
		if (req.readyState == 4) return;
		req.send(postData);
	}
	
} // End Obj REQ
;
