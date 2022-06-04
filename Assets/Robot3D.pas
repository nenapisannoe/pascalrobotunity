{$reference 'System.dll'}
unit Robot3D;

interface


procedure ConnectToUnity();
procedure GoRight();
procedure GoUp();
procedure GoDown();
procedure GoLeft();
procedure GoRightUntilWall();
procedure GoDownUntilWall();
procedure GoLeftUntilWall();
procedure GoUpUntilWall();
procedure SendString(s: string);
function ReceiveString(): string;


implementation

uses System, System.Net, System.Net.Sockets, System.IO;

type
  TByteArray = array of byte;

var
  count, len: integer;
  s: string;
  stream: NetworkStream;
  client: TCPClient;
  lines: array[0..2] of string;
  bytes, buffer: TByteArray;

procedure ConnectToUnity();
begin
  var p := new System.Diagnostics.Process;
  p.StartInfo.FileName := 'Robot.exe';
  p.Start;
  Sleep(3000);
  bytes := TByteArray(System.Array.CreateInstance(typeof(byte), 1024));
  client := TCPClient.Create;
  client.Connect('127.0.0.1', 1024);
  stream := client.GetStream;
end;

procedure SendString(s: string);
begin
  buffer := System.Text.Encoding.UTF8.GetBytes(s);
  len := buffer.Length;
  write('Sent ');
  write(len);
  write(' bytes: ');
  writeln(s);
  stream.Write(buffer, 0, len);
end;


function ReceiveString(): string;
begin
  len := stream.Read(bytes, 0, bytes.Length);
  Result := System.Text.Encoding.UTF8.GetString(bytes, 0, len);
  write('Received ');
  write(len);
  write(' bytes: ');
  writeln(Result);
end;

procedure GoRight();
begin
  var s: string;
  SendString('data');
  s := ReceiveString();
  var ar := s.Split(' ');
  if(ar[0] <> '0,5') then
    SendString('right')
  else
    SendString('die');
  Sleep(1000);
end;

procedure GoLeft();
begin
  var s: string;
  SendString('data');
  s := ReceiveString();
  var ar := s.Split(' ');
  if(ar[2] <> '0,5') then
    SendString('left')
  else
    SendString('die');
    writeln('You died!');
  Sleep(1000);
end;

procedure GoUp();
begin
  var s: string;
  SendString('data');
  s := ReceiveString();
  var ar := s.Split(' ');
  if(ar[3] <> '0,5') then
    SendString('up')
  else
    SendString('die');
    writeln('You died!');
  Sleep(1000);
end;

procedure GoDown();
begin
  var s: string;
  SendString('data');
  s := ReceiveString();
  var ar := s.Split(' ');
  if(ar[1] <> '0,5') then
    SendString('down')
  else
    SendString('die');
    writeln('You died!');
  Sleep(1000);
end;

procedure GoRightUntilWall();
begin
  var move: boolean;
  move := true;
  while(move) do
  begin
    var s: string;
    SendString('data');
    s := ReceiveString();
    var ar := s.Split(' ');
    if(ar[4] <> 'dead') then 
    begin
      if(ar[0] <> '0,5') then
        SendString('right')
      else
        move := false;
      Sleep(1000);
    end
    else
      move := false;
      writeln('You died!');
  end;
end;

procedure GoDownUntilWall();
begin
  var move: boolean;
  move := true;
  while(move) do
  begin
    var s: string;
    SendString('data');
    s := ReceiveString();
    var ar := s.Split(' ');
    if(ar[4] <> 'dead') then 
    begin
      if(ar[1] <> '0,5') then
        SendString('down')
      else
        move := false;
      Sleep(1000);
    end
    else
      move := false;
      writeln('You died!');
  end;
end;


procedure GoLeftUntilWall();
begin
  var move: boolean;
  move := true;
  while(move) do
  begin
    var s: string;
    SendString('data');
    s := ReceiveString();
    var ar := s.Split(' ');
    if(ar[4] <> 'dead') then 
    begin
      if(ar[2] <> '0,5') then
        SendString('left')
      else
        move := false;
      Sleep(1000);
    end
    else
      move := false;
      writeln('You died!');
  end;
end;

procedure GoUpUntilWall();
begin
  var move: boolean;
  move := true;
  while(move) do
  begin
    var s: string;
    SendString('data');
    s := ReceiveString();
    var ar := s.Split(' ');
    if(ar[4] <> 'dead') then 
    begin
      if(ar[3] <> '0,5') then
        SendString('up')
      else
        move := false;
      Sleep(1000);
    end
    else
      move := false;
      writeln('You died!');
  end;
end;

end.
