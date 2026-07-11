# -*- coding: utf-8 -*-
"""Threaded local static server for RMR UI option previews."""
from __future__ import print_function
import http.server
import socketserver
import os
import sys
import traceback

DIR = os.path.dirname(os.path.abspath(__file__))
os.chdir(DIR)
PORT = int(sys.argv[1]) if len(sys.argv) > 1 else 8765
HOST = "127.0.0.1"


class Handler(http.server.SimpleHTTPRequestHandler):
    extensions_map = {
        **getattr(http.server.SimpleHTTPRequestHandler, "extensions_map", {}),
        ".html": "text/html; charset=utf-8",
        ".js": "application/javascript; charset=utf-8",
        ".css": "text/css; charset=utf-8",
        ".json": "application/json; charset=utf-8",
    }

    def do_GET(self):
        if self.path in ("/", "/index.html"):
            if os.path.isfile("atlas_ui.html"):
                self.path = "/atlas_ui.html"
            elif os.path.isfile("realization_floor_ui.html"):
                self.path = "/realization_floor_ui.html"
            elif os.path.isfile("index.html"):
                self.path = "/index.html"
        return http.server.SimpleHTTPRequestHandler.do_GET(self)

    def do_POST(self):
        n = int(self.headers.get("Content-Length", 0) or 0)
        body = self.rfile.read(n) if n else b"{}"
        if self.path == "/select":
            path = "selection.json"
        elif self.path in ("/select_realization_ui", "/select_realization"):
            path = "selection_realization_ui.json"
        elif self.path in ("/select_atlas_ui", "/select_atlas"):
            path = "selection_atlas_ui.json"
        else:
            self.send_error(404, "Unknown POST path")
            return
        with open(path, "wb") as f:
            f.write(body)
        self.send_response(200)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Access-Control-Allow-Origin", "*")
        self.end_headers()
        self.wfile.write(b'{"ok":true}')

    def end_headers(self):
        self.send_header("Cache-Control", "no-store, no-cache, must-revalidate")
        self.send_header("Access-Control-Allow-Origin", "*")
        http.server.SimpleHTTPRequestHandler.end_headers(self)

    def log_message(self, fmt, *args):
        sys.stderr.write("[ui] " + (fmt % args) + "\n")
        sys.stderr.flush()


class ThreadingHTTPServer(socketserver.ThreadingMixIn, socketserver.TCPServer):
    allow_reuse_address = True
    daemon_threads = True


def main():
    try:
        httpd = ThreadingHTTPServer((HOST, PORT), Handler)
    except OSError as e:
        print("FAILED to bind %s:%s — %s" % (HOST, PORT, e), flush=True)
        print("Try: python serve_http.py 8766", flush=True)
        sys.exit(1)

    print("READY http://%s:%s/" % (HOST, PORT), flush=True)
    print("Atlas UI:        http://%s:%s/atlas_ui.html" % (HOST, PORT), flush=True)
    print("Realization UI:  http://%s:%s/realization_floor_ui.html" % (HOST, PORT), flush=True)
    print("Serving dir: %s" % DIR, flush=True)
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        print("\nStopped.", flush=True)
    except Exception:
        traceback.print_exc()
        sys.exit(1)


if __name__ == "__main__":
    main()
