Imports System.Console

Module Module1
    Public Version As String, Roberto As Player, Kurxinyan As Enemy, playerUi As New UI(), i As Integer, AmmoBoxes, HealthBoxes As New List(Of DropUp), time As ULong, pos, state As Integer
    Public sett As New Settings(New Player(10, WindowHeight - 5, "R", 60, 0), New Enemy(2, 2, "K", 1000), Difficulty.Easy)
    Sub Main()
        Dim Hadwon As Integer
        Version = "Running Robert in Ruins| Alpha v0.1 "
        pos = 1
        CursorVisible = False
        SetWindow()
        Clear()
        DrawCursor(pos)
        Draw_Menu()
        While True
            If KeyAvailable Then
                Draw_Menu()
                Select Case ReadKey(True).Key
                    Case ConsoleKey.DownArrow
                        If pos <> 4 Then
                            ClearCursor(pos)
                            pos += 1
                        End If
                    Case ConsoleKey.UpArrow
                        If pos <> 1 Then
                            ClearCursor(pos)
                            pos -= 1
                        End If
                    Case ConsoleKey.Enter
                        Select Case pos
                            Case 1
                                Clear()
                                Hadwon = GameLoop(sett)
                                If Hadwon = 1 Then
                                    Dialog("You Won", False)
                                    System.Threading.Thread.Sleep(2500)
                                ElseIf Hadwon = 0 Then
                                    Dialog("Game Over", False)
                                    System.Threading.Thread.Sleep(2500)
                                End If
                            Case 2
                                Clear()
                                sett = Set_Settings()
                            Case 3
                                About()
                            Case 4
                                Return
                        End Select
                        Clear()
                        Draw_Menu()
                End Select
                DrawCursor(pos)
            End If
        End While
    End Sub

    Public Sub SetWindow()
        Title = Version
        SetBufferSize(1000, 1000)
        SetWindowSize(100, 40)
        SetBufferSize(WindowWidth, WindowHeight)
    End Sub
    Public Function Set_Settings()
        Clear()
        pos = 1
        DrawCursor(pos)

        While True
            Draw_Settings()
            If KeyAvailable Then
                Select Case ReadKey(True).Key
                    Case ConsoleKey.DownArrow
                        If pos <> 3 Then
                            ClearCursor(pos)
                            pos += 1
                        End If
                    Case ConsoleKey.UpArrow
                        If pos <> 1 Then
                            ClearCursor(pos)
                            pos -= 1
                        End If
                    Case ConsoleKey.Enter
                        Select Case pos
                            Case 1
                                sett = Set_Difficulty()
                                pos = 1
                            Case 2
                                Return New Settings(New Player(10, WindowHeight - 5, "R", 0, 0), New Enemy(15, WindowHeight - 5, "K", 1), Difficulty.Tutorial)
                            Case 3
                                Return sett
                        End Select
                End Select
                DrawCursor(pos)
            End If
        End While
    End Function
    Public Function Set_Difficulty()
        Clear()
        pos = 1
        DrawCursor(pos)
        While True
            Draw_Difficulty()
            If KeyAvailable Then
                Select Case ReadKey(True).Key
                    Case ConsoleKey.DownArrow
                        If pos <> 4 Then
                            ClearCursor(pos)
                            pos += 1
                        End If
                    Case ConsoleKey.UpArrow
                        If pos <> 1 Then
                            ClearCursor(pos)
                            pos -= 1
                        End If
                    Case ConsoleKey.Enter
                        Clear()
                        Select Case pos
                            Case 1
                                Return New Settings(New Player(10, WindowHeight - 5, "R", 60, 0), New Enemy(2, 2, "K", 1000), Difficulty.Easy)
                            Case 2
                                Return New Settings(New Player(10, WindowHeight - 5, "R", 60, 0), New Enemy(2, 2, "K", 3000), Difficulty.Normal)
                            Case 3
                                Return New Settings(New Player(10, WindowHeight - 5, "R", 60, 0), New Enemy(2, 2, "K", 7000), Difficulty.Hard)
                            Case 4
                                Return New Settings(New Player(10, WindowHeight - 5, "S", 60, 0), New Enemy(2, 2, "K", 15000), Difficulty.Shrek)
                        End Select
                End Select
                DrawCursor(pos)
            End If
        End While
    End Function

    Public Function GameLoop(ByVal Setting As Settings)
#Region "Setup"
        Title = "Running Robert in Ruins| Boss Fight "
        Roberto = New Player(Setting.playerstats.X, Setting.playerstats.Y, Setting.playerstats.character, Setting.playerstats.Bullet_Amount, Setting.playerstats.Bullet_Count)
        Roberto.Health = Setting.playerstats.Health
        Roberto.IsDead = False
        HealthBoxes = New List(Of DropUp)
        AmmoBoxes = New List(Of DropUp)

        Kurxinyan = New Enemy(Setting.enemystats.X, Setting.enemystats.Y, Setting.enemystats.character, Setting.enemystats.Health)
        Dim speed, updatesp, dropspeed As Integer

        Select Case sett.difficulty
            Case Difficulty.Easy
                speed = 5
                updatesp = 60
                dropspeed = 1500
            Case Difficulty.Normal
                speed = 15
                updatesp = 20
                dropspeed = 15000
            Case Difficulty.Hard
                speed = 10
                updatesp = 40
                dropspeed = 7000
            Case Difficulty.Shrek
                speed = 20
                updatesp = 10
                dropspeed = 45000
        End Select
#End Region
        If sett.difficulty <> Difficulty.Tutorial Then
#Region "Dialog"
            i = Dialog("Kurxinyan:: Robert, Du Samvely kardacel es?", True, "Robert:: Che ha", "Robert:: Samvely ova")
            Clear()
            If i = 1 Then
                Dialog("Kurxinyan:: Erkus!!!!", False)
            Else
                Dialog("Kurxinyan:: To Anasuuuun!!!", False)
            End If
            ReadKey(True)
            Clear()
#End Region
            'Game Loop
            While Not Roberto.IsDead
                CursorVisible = False
                If KeyAvailable Then
                    Roberto.Move()
                End If

                Draw()
                Check()
                DrawBoxes()
                Roberto.DrawBullets()
                Dim shoot As Boolean
                If time Mod updatesp = 0 Then
                    shoot = True
                Else
                    shoot = False
                End If
                If time Mod 20 = 0 Then
                    Kurxinyan.Move(shoot)
                End If
                Kurxinyan.DrawBullets()
                If time Mod 1200 = 0 Then
                    CreateDropups(1, DropUpType.Ammo_Box)
                End If
                If time Mod dropspeed = 0 Then
                    CreateDropups(1, DropUpType.Health_Box)
                End If
                If time Mod dropspeed = 0 Then
                    Roberto.Health -= 0.5
                End If
                If Kurxinyan.IsDead Then
                    Roberto.IsDead = True
                End If

                Threading.Thread.Sleep(15)
                time += speed

            End While
            Clear()
            If Roberto.IsDead And Kurxinyan.IsDead Then
                Return 1
            Else
                Return 0
            End If
        Else
            Tutorial()
            Return 2
        End If
    End Function

    Public Sub Tutorial()
        Title = "Running Robert in Ruins| Test Chamber"
#Region "Robert"
        SetCursorPosition(5, 3)
        Write(" | R < This is your charachter, his name is Robert and he is")
        SetCursorPosition(5, 4)
        Write(" |     READY TO KILL his TEACHER ")
        SetCursorPosition(5, 6)
        Write(" |     Press -> To Continue (Left Arrow)")
Roberto:
        If ReadKey(True).Key = ConsoleKey.RightArrow Then
            Clear()
        Else
            GoTo Roberto
        End If
#End Region
#Region "Kurxinyan"
        SetCursorPosition(5, 3)
        Write(" | K < This is your enemy, her name is Shushanik Kurxinyan and she is")
        SetCursorPosition(5, 4)
        Write(" |     READY TO KILL his Student ")
        SetCursorPosition(5, 6)
        Write(" |     Press -> To Continue (Left Arrow)")
Kurxinyan:
        If ReadKey(True).Key = ConsoleKey.RightArrow Then
            Clear()
        Else
            GoTo Kurxinyan
        End If
#End Region
#Region "Goal"

        SetCursorPosition(5, 3)
        Write(" | The story is that Robert hasn't read Samvel novel and")
        SetCursorPosition(5, 4)
        Write(" | He will be KILLED by his TEACHER.")
        SetCursorPosition(5, 5)
        Write(" | Your Goal is to help Robert survive.")
        SetCursorPosition(5, 7)
        Write(" | Press -> To Continue (Left Arrow)")
Goal:
        If ReadKey(True).Key = ConsoleKey.RightArrow Then
            Clear()
        Else
            GoTo Goal
        End If
#End Region
#Region "Walk"
        SetCursorPosition(5, 3)
        Write(" | ╔══════╗ You can move your character using Arrow Keys. ")
        SetCursorPosition(5, 4)
        Write(" | ║      ║")
        SetCursorPosition(5, 5)
        Write(" | ║      ║")
        SetCursorPosition(5, 6)
        Write(" | ║      ║ To Shoot, you have to use W,A,S,D keys.(Not working in Test Chamber) ")
        SetCursorPosition(5, 7)
        Write(" | ╚══════╝ Continue (Space)")
        Dim key = ConsoleKey.Sleep
        Roberto.X = 11
        Roberto.Y = 5
        While key <> ConsoleKey.Spacebar
            If KeyAvailable Then
                key = ReadKey(True).Key
                Roberto.Move_Tutorial(key)
            End If
            Roberto.Draw()
        End While
        Clear()
#End Region
#Region "PickUps"
        SetCursorPosition(5, 3)
        Write(" | ╔══════╗ Those Are PickUps ")
        SetCursorPosition(5, 4)
        Write(" | ║      ║ There are two types of Pickups")
        SetCursorPosition(5, 5)
        Write(" | ║      ║ H <- Health Pickup (gives player 20 health)")
        SetCursorPosition(5, 6)
        Write(" | ║      ║ P <- Bullet Pickup (gives player 60 bullets)")
        SetCursorPosition(5, 7)
        Write(" | ╚══════╝ ")
        SetCursorPosition(5, 9)
        Write("            In Top Left, there is your Bullet counter,")
        SetCursorPosition(5, 10)
        Write("            You can Reload your gun, pressing R button.")
        SetCursorPosition(5, 12)
        Write("            In Top Right, there is your Health counter,")
        SetCursorPosition(5, 13)
        Write("            You can Heal your player, picking up dropups.")
        SetCursorPosition(5, 14)
        Write("            Finish Tutorial (Enter)")
        key = ConsoleKey.Sleep
        Roberto.X = 11
        Roberto.Y = 5
        AmmoBoxes.Add(New DropUp(9, 4, DropUpType.Ammo_Box))
        HealthBoxes.Add(New DropUp(14, 6, DropUpType.Health_Box))
        While key <> ConsoleKey.Enter
            If KeyAvailable Then
                key = ReadKey(True).Key
                Roberto.Move_Tutorial(key)
            End If
            Roberto.Draw()
            Roberto.Check()
            DrawBoxes()
            playerUi.Draw(Roberto, Difficulty.Shrek)
        End While
        Clear()
#End Region
    End Sub
    Public Sub About()
        MsgBox("Currently is not working...", MsgBoxStyle.Critical, "About")
    End Sub

    Public Function Draw_Menu()
        Dim temp As String
        temp = "Menu"
        ForegroundColor = ConsoleColor.White
        SetCursorPosition((WindowWidth - temp.Length) / 2, (WindowHeight) / 2 - 3)
        Write(temp)
        ForegroundColor = ConsoleColor.Gray
        temp = "Start game"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2 - 1)
        Write(temp)
        temp = "Settings"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2)
        Write(temp)
        temp = "About"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2 + 1)
        Write(temp)
        temp = "Exit"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2 + 2)
        Write(temp)
        Return 0
    End Function
    Public Function Draw_Settings()
        Dim temp As String
        temp = "Settings"
        ForegroundColor = ConsoleColor.White
        SetCursorPosition((WindowWidth - temp.Length) / 2, (WindowHeight) / 2 - 3)
        Write(temp)
        ForegroundColor = ConsoleColor.Gray
        temp = "Difficulty"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2 - 1)
        Write(temp)
        temp = "Tutorial"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2)
        Write(temp)
        temp = "Exit"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2 + 1)
        Write(temp)
        Return 0
    End Function
    Public Function Draw_Difficulty()
        Dim temp As String
        temp = "Difficulty"
        ForegroundColor = ConsoleColor.White
        SetCursorPosition((WindowWidth - temp.Length) / 2, (WindowHeight) / 2 - 3)
        Write(temp)
        ForegroundColor = ConsoleColor.Gray
        temp = "Easy"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2 - 1)
        Write(temp)
        temp = "Normal"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2)
        Write(temp)
        temp = "Hard"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2 + 1)
        Write(temp)
        temp = "Shrek"
        SetCursorPosition((WindowWidth) / 2 - 5, (WindowHeight) / 2 + 2)
        Write(temp)
        Return 0
    End Function
    Public Function Draw_Pause()
        SetCursorPosition(5, 3)
        Write(" ╔═════════════════════════════╗ ")
        SetCursorPosition(5, 4)
        Write(" ║          Pause Menu         ║")
        SetCursorPosition(5, 5)
        Write(" ║  Press Escape to Continue.  ║")
        SetCursorPosition(5, 6)
        Write(" ╚═════════════════════════════╝ ")
        Return 0
    End Function

    Public Sub DrawCursor(ByVal place As Integer)
        SetCursorPosition((WindowWidth) / 2 - 7, (WindowHeight) / 2 + (place - 2))
        ForegroundColor = ConsoleColor.Yellow
        Write(">")
        ForegroundColor = ConsoleColor.White
    End Sub
    Public Sub ClearCursor(ByVal place As Integer)
        SetCursorPosition((WindowWidth) / 2 - 7, (WindowHeight) / 2 + (place - 2))
        Write(" ")
    End Sub

    Public Function Dialog(ByVal Question As String, isAskable As Boolean, Optional answer1 As String = "", Optional answer2 As String = "")
        If isAskable Then
            SetCursorPosition((WindowWidth - Question.Length) / 2, WindowHeight / 2 - 1)
            Write(Question)

            SetCursorPosition((WindowWidth - Question.Length) / 2 + 1, WindowHeight / 2)
            Write("(D1) " + answer1)
            SetCursorPosition((WindowWidth - Question.Length) / 2 + 1, WindowHeight / 2 + 1)
            Write("(D2) " + answer2)
            Dim temp As ConsoleKey
            temp = ReadKey(True).Key
            While temp <> ConsoleKey.D1 And temp <> ConsoleKey.D2
                temp = ReadKey(True).Key
            End While
            If temp = ConsoleKey.D1 Then
                Return 1
            End If
            Return 2
        Else
            SetCursorPosition((WindowWidth - Question.Length) / 2, WindowHeight / 2)
            Write(Question)
            SetCursorPosition((WindowWidth - "Press any key to start".Length) / 2, WindowHeight / 2 + 1)
            Write("Press any key to start")
            Return 0
        End If
    End Function

    Public Function getRandNum(ByVal min As Double, ByVal max As Double)
        Return CInt((Rnd() * ((max - min) + 1)) + min)
    End Function
    Public Function CreateDropups(ByVal count As Integer, ByVal IsAmmoBox As Boolean)
        For i = 1 To count
            If IsAmmoBox Then
                AmmoBoxes.Add(New DropUp(getRandNum(5, WindowWidth - 5), getRandNum(5, WindowHeight - 5), DropUpType.Ammo_Box))
            Else
                HealthBoxes.Add(New DropUp(getRandNum(5, WindowWidth - 5), getRandNum(5, WindowHeight - 5), DropUpType.Health_Box))
            End If
        Next i
        Return 0
    End Function

    Public Sub Check()
        Kurxinyan.Check()
        Roberto.Check()
        Kurxinyan.Change_Direction()
    End Sub
    Public Sub Draw()
        Roberto.Draw()
        Kurxinyan.Draw()
        playerUi.Draw(Roberto, sett.difficulty)
    End Sub
    Public Sub Pause()
        Draw_Pause()
        While ReadKey(True).Key <> ConsoleKey.Escape
        End While
    End Sub

    Public Class Player
        Public X, Y, Bullet_Amount, Bullet_Count As Integer, character As Char, key, newKey As ConsoleKey, Health As Double
        Public bullets As List(Of Bullet)
        Public IsDead As Boolean
        Public Sub New(ByVal playerX As Integer, playerY As Integer, icon As Char, bulletamount As Integer, bulletcount As Integer)

            Bullet_Amount = bulletamount
            Bullet_Count = bulletcount
            bullets = New List(Of Bullet)
            character = icon
            Health = 50
            IsDead = False
            X = playerX
            Y = playerY
        End Sub
        Public Sub Draw()
            SetCursorPosition(X, Y)
            Write(character)
        End Sub
        Public Sub Move()
            newKey = ReadKey(True).Key
            SetCursorPosition(X, Y)
            Write(" ")
            Select Case newKey
                Case ConsoleKey.RightArrow
                    If X + 1 < WindowWidth - 1 Then
                        X = X + 1
                    End If
                    key = newKey
                Case ConsoleKey.LeftArrow
                    If X - 1 > 0 Then
                        X = X - 1
                    End If
                    key = newKey
                Case ConsoleKey.UpArrow
                    If Y - 1 > 0 Then
                        Y = Y - 1
                    End If
                    key = newKey
                Case ConsoleKey.DownArrow
                    If Y + 1 < WindowHeight - 1 Then
                        Y = Y + 1
                    End If
                    key = newKey
                Case ConsoleKey.W
                    Shoot(ConsoleKey.UpArrow)
                Case ConsoleKey.Escape
                    Pause()
                    Clear()
                Case ConsoleKey.A
                    Shoot(ConsoleKey.LeftArrow)
                Case ConsoleKey.S
                    Shoot(ConsoleKey.DownArrow)
                Case ConsoleKey.D
                    Shoot(ConsoleKey.RightArrow)
                Case ConsoleKey.R
                    Reload()
            End Select

        End Sub
        Public Sub Shoot(ByVal Dir As ConsoleKey)
            If Bullet_Count > 0 Then
                bullets.Add(New Bullet(X, Y, Dir))
                Bullet_Count = Bullet_Count - 1
            End If
        End Sub
        Public Sub Reload()
            If Bullet_Amount <> 0 Then
                Bullet_Amount = Bullet_Amount - (30 - Bullet_Count)
                Bullet_Count = 30
                If Bullet_Amount < 0 Then
                    Bullet_Count = Bullet_Count + Bullet_Amount
                    Bullet_Amount = 0
                End If
            End If
            'If temp > Bullet_Amount Then
            '    Bullet_Count = Bullet_Amount
            '    Bullet_Amount = 0
            'Else
            '    Bullet_Count = 30
            '    Bullet_Amount = Bullet_Amount - temp
            'End If


        End Sub
        Public Sub Check()
            Dim DeleteList As New List(Of DropUp)
            For Each Drop In AmmoBoxes
                If X = Drop.X And Y = Drop.Y Then
                    Bullet_Amount += 60
                    If Bullet_Amount > 666 Then
                        Bullet_Amount = 666
                    End If
                    DeleteList.Add(Drop)
                End If
            Next Drop

            For Each item In DeleteList
                AmmoBoxes.Remove(item)
            Next item

            DeleteList = New List(Of DropUp)

            For Each Drop In HealthBoxes
                If X = Drop.X And Y = Drop.Y Then
                    Health += 20
                    If Health > 100 Then
                        Health = 100
                    End If
                    DeleteList.Add(Drop)
                End If
            Next Drop

            For Each item In DeleteList
                HealthBoxes.Remove(item)
            Next item

            If X = Kurxinyan.X And Y = Kurxinyan.Y Then
                Health -= 50
            End If

            Dim temp As New List(Of Bullet)

            For Each bullet In Kurxinyan.bullets
                If bullet.X = X And bullet.Y = Y Then
                    Health -= 5
                    temp.Add(bullet)
                End If
            Next
            For Each bullet In temp
                Kurxinyan.bullets.Remove(bullet)
            Next
            If Int(Health) < 0 Then
                IsDead = True
            End If

        End Sub
        Public Sub DrawBullets()
            Try
                For Each bullet In bullets
                    Try
                        bullet.ChangeCoords()
                        bullet.Draw()
                    Catch
                        bullets.Remove(bullet)
                    End Try
                Next
            Catch
            End Try
        End Sub
        Public Sub Move_Tutorial(ByVal keys As ConsoleKey)
            newKey = keys
            SetCursorPosition(X, Y)
            Write(" ")
            Select Case newKey
                Case ConsoleKey.RightArrow
                    If X + 1 < 15 Then
                        X = X + 1
                    End If
                    key = newKey
                Case ConsoleKey.LeftArrow
                    If X - 1 > 8 Then
                        X = X - 1
                    End If
                    key = newKey
                Case ConsoleKey.UpArrow
                    If Y - 1 > 3 Then
                        Y = Y - 1
                    End If
                    key = newKey
                Case ConsoleKey.DownArrow
                    If Y + 1 < 7 Then
                        Y = Y + 1
                    End If
                    key = newKey
                Case ConsoleKey.R
                    Reload()
            End Select
        End Sub
    End Class
    Public Class Enemy
        Public X, Y, prevX, prevY As Integer, character As Char, direction As ConsoleKey, Health As Integer, bullets As List(Of Bullet), state As Integer

        Public Sub New(ByVal enemyX As Integer, enemyY As Integer, icon As Char, helth As Integer)
            bullets = New List(Of Bullet)
            character = icon
            Health = helth
            X = enemyX
            Y = enemyY
            Change_Direction()
        End Sub
        Public Sub Check()
            Dim temp As New List(Of Bullet)
            For Each bullet In Roberto.bullets
                If bullet.X = X And bullet.Y = Y Then
                    Health -= 35
                    temp.Add(bullet)
                End If
            Next bullet
            For Each bullet In temp
                Roberto.bullets.Remove(bullet)
            Next
        End Sub
        Public Sub Move(ByVal shootable As Boolean)
            prevX = X
            prevY = Y
            Select Case direction
                Case ConsoleKey.RightArrow
                    If X + 1 < WindowWidth - 1 Then
                        X = X + 1
                    End If
                    If shootable Then
                        bullets.Add(New Bullet(X, Y, ConsoleKey.UpArrow))
                    End If
                Case ConsoleKey.LeftArrow
                    If X - 1 > 0 Then
                        X = X - 1
                    End If
                    If shootable Then
                        bullets.Add(New Bullet(X, Y, ConsoleKey.DownArrow))
                    End If
                Case ConsoleKey.UpArrow
                    If Y - 1 > 0 Then
                        Y = Y - 1
                    End If
                    If shootable Then
                        bullets.Add(New Bullet(X, Y, ConsoleKey.LeftArrow))
                    End If
                Case ConsoleKey.DownArrow
                    If Y + 1 < WindowHeight - 1 Then
                        Y = Y + 1
                    End If
                    If shootable Then
                        bullets.Add(New Bullet(X, Y, ConsoleKey.RightArrow))
                    End If
            End Select
        End Sub
        Public Sub Change_Direction()
            If (Y = 2 And X = 2) Then
                direction = ConsoleKey.DownArrow
            ElseIf (Y = WindowHeight - 3 And X = 2) Then
                direction = ConsoleKey.RightArrow
            ElseIf (Y = WindowHeight - 3 And X = WindowWidth - 3) Then
                direction = ConsoleKey.UpArrow
            ElseIf (Y = 2 And X = WindowWidth - 3) Then
                direction = ConsoleKey.LeftArrow
            End If
        End Sub
        Public Sub Check_Bullets()
            Dim temp As New List(Of Bullet)
            For Each bullet In bullets
                If bullet.X > WindowWidth Or bullet.Y > WindowHeight Or bullet.X < 0 Or bullet.Y < 0 Then
                    temp.Add(bullet)
                Else
                    If bullet.X = Roberto.X Or bullet.Y = Roberto.Y Then
                        Roberto.Health -= 10
                    End If
                End If
            Next bullet
            For Each bullet In temp
                bullets.Remove(bullet)
            Next
        End Sub
        Public Sub Draw()
            SetCursorPosition(prevX, prevY)
            Write(" ")
            SetCursorPosition(X, Y)
            Write(character)
        End Sub
        Public Sub DrawBullets()
            Try
                For Each bullet In bullets
                    Try
                        bullet.ChangeCoords()
                        bullet.Draw()
                    Catch
                        bullets.Remove(bullet)
                    End Try
                Next
            Catch
            End Try
        End Sub
        Public Function IsDead()
            If Health <= 0 Then
                Return True
            End If
            Return False
        End Function
    End Class

    Public Class UI
        Public Sub Draw(playerstat As Player, diff As Difficulty)
            SetCursorPosition(1, 0)
            Write("Bullets : {0}/{1}   ", playerstat.Bullet_Amount, playerstat.Bullet_Count)
            Dim temp As String
            temp = ("Health : " + CInt(playerstat.Health).ToString())
            SetCursorPosition(WindowWidth - (temp.Length + 1), 0)
            Write(temp)
            Title = "Running Robert in Ruins| " + diff.ToString() + " Boss Fight "
        End Sub
    End Class

    Public Class Bullet
        Public Property X() As Integer
        Public Property Y() As Integer
        Public Property Direction As ConsoleKey

        Public Sub New(ByVal posx As Integer, ByVal posy As Integer, ByVal Dir As ConsoleKey)
            X = posx
            Y = posy
            Direction = Dir
        End Sub
        Public Sub ChangeCoords()
            Select Case Direction
                Case ConsoleKey.UpArrow
                    Y = Y - 1
                Case ConsoleKey.DownArrow
                    Y = Y + 1
                Case ConsoleKey.LeftArrow
                    X = X - 1
                Case ConsoleKey.RightArrow
                    X = X + 1
            End Select
        End Sub
        Public Sub Draw()
            Select Case Direction
                Case ConsoleKey.UpArrow
                    SetCursorPosition(X, Y + 1)
                Case ConsoleKey.DownArrow
                    SetCursorPosition(X, Y - 1)
                Case ConsoleKey.LeftArrow
                    SetCursorPosition(X + 1, Y)
                Case ConsoleKey.RightArrow
                    SetCursorPosition(X - 1, Y)
            End Select
            Write(" ")
            SetCursorPosition(X, Y)
            Write("o")
        End Sub

    End Class
    Public Class DropUp
        Public X, Y As Integer, DropupType As DropUpType
        Public Sub New(posX As Integer, posY As Integer, type As DropUpType)
            X = posX
            Y = posY
            DropupType = type
        End Sub
        Public Sub Draw()
            SetCursorPosition(X, Y)
            If DropupType = DropUpType.Ammo_Box Then
                Write("P")
            Else
                Write("H")
            End If
        End Sub
    End Class
    Public Sub DrawBoxes()
        For Each Drop In AmmoBoxes
            Drop.Draw()
        Next Drop

        For Each Drop In HealthBoxes
            Drop.Draw()
        Next Drop
    End Sub

    Public Class Settings
        Public playerstats As Player, enemystats As Enemy, difficulty
        Public Sub New(ByVal pstats As Player, enstats As Enemy, diff As Difficulty)
            playerstats = pstats
            enemystats = enstats
            difficulty = diff
        End Sub
    End Class

    Public Enum Difficulty
        Easy
        Normal
        Hard
        Shrek
        Tutorial
    End Enum
    Public Enum DropUpType
        Health_Box
        Ammo_Box
    End Enum

End Module