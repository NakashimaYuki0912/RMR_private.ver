import json, http.server, socketserver, os
from urllib.parse import urlparse
DIR = r"D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\tools\ui_options"
os.chdir(DIR)
PORT = 8765
class H(http.server.SimpleHTTPRequestHandler):
    def do_POST(self):
        if self.path != "/select":
            self.send_error(404); return
        n = int(self.headers.get("Content-Length", 0))
        body = self.rfile.read(n)
        with open("selection.json", "wb") as f:
            f.write(body)
        self.send_response(200)
        self.send_header("Content-Type", "application/json")
        self.end_headers()
        self.wfile.write(b'{"ok":true}')
    def log_message(self, fmt, *args):
        print("[ui]", fmt % args)
with socketserver.TCPServer(("127.0.0.1", PORT), H) as httpd:
    print(f"UI options: http://127.0.0.1:{PORT}/")
    httpd.serve_forever()
